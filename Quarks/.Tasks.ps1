Include-PluginScripts

task SetVersion

task Pack {
	New-Item $artifactsPath -Type directory -Force | Out-Null
	Get-ChildItem $basePath\$projectName\*.nuspec -Recurse | %{ exec { & NuGet pack $_.FullName -OutputDirectory $artifactsPath } }
}

task . Clean, Compile, Test, Pack