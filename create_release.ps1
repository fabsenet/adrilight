$version = gc .\version.txt

del ".\Release\adrilight_$version.zip" -Force -ErrorAction SilentlyContinue
rm ".\Release\adrilight_$version" -Force -Recurse -ErrorAction SilentlyContinue
md ".\Release\adrilight_$version\PC" -Force

#find msbuild
$_decSep = [System.Threading.Thread]::CurrentThread.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator;
$msbuild = $(Get-ChildItem -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\MSBuild\ToolsVersions\" | 
    Where { $_.Name -match '\\\d+.\d+$' } | 
    Sort-Object -property  @{Expression={[System.Convert]::ToDecimal($_.Name.Substring($_.Name.LastIndexOf("\") + 1).Replace(".",$_decSep).Replace(",",$_decSep))}} -Descending |
    Select-Object -First 1).GetValue("MSBuildToolsPath") + "msbuild.exe"
    
if((Test-Path -Path $msbuild) -eq $false) {Write-Error "failed to find msbuild file"; exit}

& $msbuild /m .\adrilight.sln /t:Clean /p:Configuration=Release
& $msbuild /m .\adrilight.sln /t:Rebuild /p:Configuration=Release

dir Arduino -Recurse | Copy -Destination ".\Release\adrilight_$version\Arduino\"
dir .\adrilight\bin\Release -Exclude "*.pdb","*vshost*","logs" | Copy -Destination ".\Release\adrilight_$version\PC\"

Compress-Archive -Path ".\Release\adrilight_$version\*" -DestinationPath ".\Release\adrilight_$version.zip" -Force -CompressionLevel Optimal

write-host
Write-Host "adrilight_$version.zip created!" -ForegroundColor Green
Write-Host "full path:" -ForegroundColor Gray
Write-Host (dir ".\Release\adrilight_$version.zip").FullName -ForegroundColor Yellow
