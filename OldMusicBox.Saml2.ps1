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
	<repository type="git" url="https://github.com/wzychla/OldMusicBox.Saml2.git" />
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
	<license type="expression">MIT</license>
    <description>OldMusicBox.Saml2. Independent SAML2 implementation.</description>
    <copyright>Copyright 2020 Wiktor Zychla</copyright>
	<icon>images\icon.png</icon>
    <tags>SAML2</tags>
  </metadata>
  <files>
	<file src="lib/net462/OldMusicBox.Saml2.dll" target="lib/net462/OldMusicBox.Saml2.dll" />  
	<file src="..\..\..\icon.png" target="images\" />
  </files>
</package>
"@

$contents = $contents -replace "__VERSION", $version

New-Item -ItemType File -Path $nuspec -value $contents

# spakuj
Set-Location -Path $path
nuget pack

Set-Location -Path "../../.."