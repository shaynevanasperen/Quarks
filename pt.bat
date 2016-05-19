@echo off
powershell -NoProfile -ExecutionPolicy Unrestricted -Command ^
$ErrorActionPreference = 'Stop'; ^
if (!(Get-Command NuGet -ErrorAction SilentlyContinue) -and !(Test-Path '%LocalAppData%\NuGet\NuGet.exe')) { ^
	Write-Host 'Downloading NuGet.exe'; ^
	(New-Object system.net.WebClient).DownloadFile('https://dist.nuget.org/win-x86-commandline/latest/nuget.exe', '%LocalAppData%\NuGet\NuGet.exe'); ^
} ^
if (Test-Path '%LocalAppData%\NuGet\NuGet.exe') { ^
	Set-Alias NuGet (Resolve-Path %LocalAppData%\NuGet\NuGet.exe); ^
} ^
Write-Host 'Restoring NuGet packages'; ^
NuGet restore; ^
. '.\packages\PowerTasks.3.1.0\Functions.ps1'; ^
$basePath = Resolve-Path .; ^
Set-Location '.\Quarks'; ^
$basePath = Split-Path (Split-Path (Resolve-Path $basePath -Relative)); ^
$projectName = 'Quarks'; ^
$packagesPath = '..\packages'; ^
$invokeBuildPath = Get-RequiredPackagePath Invoke-Build $basePath\$projectName; ^
& $invokeBuildPath\tools\Invoke-Build.ps1 %* -File .Tasks.ps1;
exit /b %ERRORLEVEL%
