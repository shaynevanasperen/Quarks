Include-PluginScripts

task SetVersion

task Pack Test, {
	New-Item $buildsPath -Type directory -Force | Out-Null
	Get-ChildItem $basePath\$projectName\*.nuspec -Recurse | %{ exec { & NuGet pack $_.FullName -OutputDirectory $buildsPath } }
}

task . Pack