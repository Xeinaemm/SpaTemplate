 while ( Test-Path -Path ./dist -ErrorAction Ignore )
 {
    Remove-Item ./dist -Force -Recurse -ErrorAction Ignore
 }
