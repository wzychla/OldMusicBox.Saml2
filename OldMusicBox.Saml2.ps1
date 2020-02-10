# /bin/release
$dll462  = Resolve-Path "./OldMusicBox.Saml2/bin/Release/OldMusicBox.Saml2.dll"
$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($dll462).FileVersion

# utwórz folder
$path = ("./OldMusicBox.Saml2/bin/" + $version)

New-Item -ItemType Directory -Force -Path $path
New-Item -ItemType Directory -Force -Path ($path + "/lib")
New-Item -ItemType Directory -Force -Path ($path + "/lib/net462")

Copy-Item $dll462 ($path + "/lib/net462")

# nuspec
$nuspec = ($path + "/OldMusicBox.Saml2.nuspec")
$contents = @"
<?xml version="1.0"?>
<package>
  <metadata>
    <id>OldMusicBox.Saml2</id>
    <version>__VERSION</version>
    <authors>wzychla</authors>
	<dependencies>
		<group targetFramework="net462">
		</group>
	</dependencies>
    <projectUrl>https://github.com/wzychla/OldMusicBox.Saml2</projectUrl>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
	<license type="expression">MIT</license>
    <description>OldMusicBox.Saml2. Independent SAML2 implementation.</description>
    <copyright>Copyright 2020 Wiktor Zychla</copyright>
    <tags>SAML2</tags>
  </metadata>
</package>
"@

$contents = $contents -replace "__VERSION", $version

New-Item -ItemType File -Path $nuspec -value $contents

# spakuj
Set-Location -Path $path
nuget pack