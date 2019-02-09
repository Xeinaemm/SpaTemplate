using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.WebDeploy;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.CI;
using System.Collections.Generic;
using Nuke.Common.Tools.Coverlet;
using System.IO;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.IO.CompressionTasks;
using static Nuke.Common.Tools.InspectCode.InspectCodeTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Tools.InspectCode;

[CheckBuildProjectConfigurations]
[DotNetVerbosityMapping]
[UnsetVisualStudioEnvironmentVariables]
[AzurePipelines(
    AzurePipelinesImage.WindowsLatest,
    InvokedTargets = new[] { nameof(Test), nameof(Pack) },
    NonEntryTargets = new[] { nameof(Restore) },
    ExcludedTargets = new[] { nameof(Clean), nameof(Coverage) })]
partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Run);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly string ProjectName = "Allegro.Api";
    [Parameter] readonly string WebDeployUsername;
    [Parameter] readonly string WebDeployPassword;
    [Parameter] readonly string WebDeployPublishUrl;
    [Parameter] readonly string WebDeploySiteName;
    [Parameter] bool IgnoreFailedSources;

    [Solution] readonly Solution Solution;
    [GitVersion] readonly GitVersion GitVersion;
    [CI] readonly AzurePipelines AzurePipelines;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath PackagesDirectory => ArtifactsDirectory / "packages";
    AbsolutePath TestResultsDirectory => ArtifactsDirectory / "test-results";
    AbsolutePath CoverageReportDirectory => ArtifactsDirectory / "coverage-report";
    AbsolutePath CoverageReportZipDirectory => ArtifactsDirectory / "coverage-report.zip";
    AbsolutePath PublishDirectory => ArtifactsDirectory / "publish";
    AbsolutePath PublishProjectDirectory => PublishDirectory / ProjectName;
    AbsolutePath InspectCodeDirectory => ArtifactsDirectory / "inspectCode.xml";

    [Partition(2)] readonly Partition TestPartition;
    IEnumerable<Project> TestProjects => TestPartition.GetCurrent(Solution.GetProjects("*.Tests"));

    Target Run => _ => _
        .DependsOn(Compile)
        .Executes(() => DotNetRun(_ => _.SetProjectFile(SourceDirectory / ProjectName / $"{ProjectName}.csproj")));

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("*/bin", "*/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution)
                .SetIgnoreFailedSources(IgnoreFailedSources));
        });

    Target Pack => _ => _
    .DependsOn(Compile)
    .Produces(PackagesDirectory / "*.nupkg")
    .Executes(() =>
    {
        DotNetPack(_ => _
            .SetProject(Solution)
            .SetNoBuild(InvokedTargets.Contains(Compile))
            .SetConfiguration(Configuration)
            .SetOutputDirectory(PackagesDirectory)
            .SetVersion(GitVersion.NuGetVersionV2));
    });

    Target Test => _ => _
    .DependsOn(Compile)
    .Produces(TestResultsDirectory / "*.trx")
    .Produces(TestResultsDirectory / "*.xml")
    .Partition(() => TestPartition)
    .Executes(() =>
    {
        DotNetTest(_ => _
            .SetConfiguration(Configuration)
            .SetNoBuild(InvokedTargets.Contains(Compile))
            .ResetVerbosity()
            .SetResultsDirectory(TestResultsDirectory)
            .When(InvokedTargets.Contains(Coverage) || IsServerBuild, _ => _
                .EnableCollectCoverage()
                .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                .SetExcludeByFile("*.Generated.cs")
                .When(IsServerBuild, _ => _
                    .EnableUseSourceLink()))
            .CombineWith(TestProjects, (_, v) => _
                .SetProjectFile(v)
                .SetLogger($"trx;LogFileName={v.Name}.trx")
                .When(InvokedTargets.Contains(Coverage) || IsServerBuild, _ => _
                    .SetCoverletOutput(TestResultsDirectory / $"{v.Name}.xml"))));

        TestResultsDirectory.GlobFiles("*.trx").ForEach(x =>
            AzurePipelines?.PublishTestResults(
                type: AzurePipelinesTestResultsType.VSTest,
                title: $"{Path.GetFileNameWithoutExtension(x)} ({AzurePipelines.StageDisplayName})",
                files: new string[] { x }));
    });

    Target Coverage => _ => _
        .DependsOn(Test)
        .Consumes(Test)
        .Produces(CoverageReportZipDirectory)
        .Executes(() =>
        {
            ReportGenerator(_ => _
                .SetReports(TestResultsDirectory / "*.xml")
                .SetReportTypes(ReportTypes.HtmlInline)
                .SetTargetDirectory(CoverageReportDirectory)
                .SetFramework("netcoreapp3.0"));

            TestResultsDirectory.GlobFiles("*.xml").ForEach(x =>
                AzurePipelines?.PublishCodeCoverage(
                    AzurePipelinesCodeCoverageToolType.Cobertura,
                    x,
                    CoverageReportDirectory));

            CompressZip(
                directory: CoverageReportDirectory,
                archiveFile: CoverageReportZipDirectory,
                fileMode: FileMode.Create);
        });

    Target Analysis => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            InspectCode(_ => _
                .SetTargetPath(Solution)
                .SetOutput(InspectCodeDirectory)
                .AddPlugin("EtherealCode.ReSpeller", InspectCodePluginLatest)
                .AddPlugin("PowerToys.CyclomaticComplexity", InspectCodePluginLatest)
                .AddPlugin("ReSharper.ImplicitNullability", InspectCodePluginLatest)
                .AddPlugin("ReSharper.SerializationInspections", InspectCodePluginLatest)
                .AddPlugin("ReSharper.XmlDocInspections", InspectCodePluginLatest));
        });

    Target Publish => _ => _
        .Requires(() => Configuration)
        .Requires(() => ProjectName)
        .Executes(() => DotNetPublish(p => p.SetWorkingDirectory(SourceDirectory / ProjectName)
            .SetConfiguration(Configuration)
            .SetOutput(PublishProjectDirectory)));

    Target Deploy => _ => _
        .DependsOn(Publish)
        .Requires(() => WebDeployUsername)
        .Requires(() => WebDeployPassword)
        .Requires(() => WebDeployPublishUrl)
        .Requires(() => WebDeploySiteName)
        .Executes(() =>
        {
            WebDeployTasks.WebDeploy(s =>
            {
                return s.SetSourcePath(PublishProjectDirectory)
                        .SetUsername(WebDeployUsername)
                        .SetPassword(WebDeployPassword)
                        .SetEnableAppOfflineRule(true)
                        .SetPublishUrl(WebDeployPublishUrl)
                        .SetSiteName(WebDeploySiteName);
            });
        });
}
