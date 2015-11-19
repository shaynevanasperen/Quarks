Include-PluginScripts

function Pack-Nuspecs ($dateModified) {
	New-Item $artifactsPath -Type directory -Force | Out-Null
	$nuspecs = Get-ChildItem $basePath\$projectName\*.nuspec -Recurse
	if ($dateModified) {
		$nuspecs = $nuspecs | Where { $_.LastWriteTime -gt $dateModified }
	}
	$nuspecs | %{ exec { & NuGet pack $_.FullName -OutputDirectory $artifactsPath } }
}

task Pack {
	Pack-Nuspecs
}

task PackRecentlyModified {
	Pack-Nuspecs (Get-Date).AddDays(-1)
}

task . Clean, Compile, Test, PackRecentlyModified, Push