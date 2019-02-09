// -----------------------------------------------------------------------
// <copyright file="CoreJob.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Web.Quartz
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using global::Quartz;
    using Xeinaemm.Quartz;

    public class CoreJob : JobBase
    {
        public override async Task PreExecuteAsync(IJobExecutionContext context)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            Debug.WriteLine(".NET Core Job invoked");
        }
    }
}
