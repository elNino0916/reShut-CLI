<#
.SYNOPSIS
    Publishes reShut CLI and builds the custom WPF installer around it.

.DESCRIPTION
    1. dotnet publish the app (framework-dependent, win-x64) into reShutCLI\bin\publish
    2. Code-signs the application binaries (if a certificate is configured)
    3. Zips that publish output into a payload the installer embeds as a resource
    4. dotnet build the Bootstrapper (net48 WPF) with the payload embedded
    5. Code-signs the installer, then copies it out as reShutCLI-<version>-setup.exe

    Requires the .NET 10 SDK (for the app) and the .NET Framework 4.8 reference
    assemblies (for the installer) - both ship with a normal Windows dev machine,
    no extra installs needed. Signing additionally needs the Windows SDK's signtool.

    Signing is opt-in. With no certificate configured the build runs exactly as
    before and produces unsigned binaries.

.PARAMETER CertificateThumbprint
    Thumbprint of a code signing certificate in CurrentUser\My or LocalMachine\My.
    Defaults to $env:RESHUT_SIGN_THUMBPRINT.

.PARAMETER CertificatePath
    Path to a .pfx instead. The password is read from $env:RESHUT_SIGN_PFX_PASSWORD.
    Defaults to $env:RESHUT_SIGN_PFX.

.EXAMPLE
    .\build-installer.ps1
    Unsigned build.

.EXAMPLE
    .\build-installer.ps1 -CertificateThumbprint A1B2C3D4E5F6...
    Signs with a certificate from the Windows certificate store.

.EXAMPLE
    $env:RESHUT_SIGN_PFX_PASSWORD = Read-Host -AsSecureString | ConvertFrom-SecureString -AsPlainText
    .\build-installer.ps1 -CertificatePath C:\keys\reshut.pfx
#>
[CmdletBinding()]
param(
    [string]$Configuration = "Release",
    [string]$CertificateThumbprint = $env:RESHUT_SIGN_THUMBPRINT,
    [string]$CertificatePath = $env:RESHUT_SIGN_PFX,
    [string]$TimestampUrl = "http://timestamp.digicert.com"
)

$ErrorActionPreference = "Stop"

. (Join-Path $PSScriptRoot "CodeSigning.ps1")

$repoRoot = Split-Path $PSScriptRoot -Parent
$appProject = Join-Path $repoRoot "reShutCLI\reShutCLI.csproj"
$publishDir = Join-Path $repoRoot "reShutCLI\bin\publish"
$bootstrapperProject = Join-Path $PSScriptRoot "Bootstrapper\Bootstrapper.csproj"
$payloadZip = Join-Path $PSScriptRoot "Bootstrapper\obj\payload.zip"
$version = "2.2.1"
$outputExe = Join-Path $PSScriptRoot "reShutCLI-$version-setup.exe"

$signing = Get-SigningConfig -Thumbprint $CertificateThumbprint -CertificatePath $CertificatePath -TimestampUrl $TimestampUrl
if ($signing) {
    Write-Host "Code signing enabled ($(if ($signing.Thumbprint) { "thumbprint $($signing.Thumbprint)" } else { Split-Path $signing.CertificatePath -Leaf }))." -ForegroundColor Cyan
}
else {
    Write-Warning "No signing certificate configured - the output will be unsigned."
}

Write-Host "Publishing reShut CLI ($Configuration, win-x64, framework-dependent)..." -ForegroundColor Cyan
if (Test-Path $publishDir) { Remove-Item -Recurse -Force $publishDir }
dotnet publish $appProject -c $Configuration -r win-x64 --self-contained false -o $publishDir
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE." }

if ($signing) {
    # Signed before packaging, because the payload zip is embedded into the installer
    # as-is - anything unsigned at this point stays unsigned on the user's disk.
    # Both files matter: reShutCLI.exe is only the native launcher stub, while the
    # managed code that actually runs lives in reShutCLI.dll.
    Write-Host "Signing application binaries..." -ForegroundColor Cyan
    Invoke-CodeSigning -Config $signing -Path @(
        (Join-Path $publishDir "reShutCLI.exe"),
        (Join-Path $publishDir "reShutCLI.dll")
    )
}

Write-Host "Packaging application payload..." -ForegroundColor Cyan
if (Test-Path $payloadZip) { Remove-Item -Force $payloadZip }
New-Item -ItemType Directory -Force -Path (Split-Path $payloadZip -Parent) | Out-Null
Compress-Archive -Path (Join-Path $publishDir "*") -DestinationPath $payloadZip -CompressionLevel Optimal

Write-Host "Building the installer (net48 WPF)..." -ForegroundColor Cyan
dotnet build $bootstrapperProject -c $Configuration --nologo "-p:PayloadZipPath=$payloadZip"
if ($LASTEXITCODE -ne 0) { throw "dotnet build (Bootstrapper) failed with exit code $LASTEXITCODE." }

$builtExe = Join-Path $PSScriptRoot "Bootstrapper\bin\$Configuration\net48\reShutCLI-Setup.exe"

if ($signing) {
    # Signed before the copy below, so the shipped exe carries the signature - and so
    # does uninstall.exe, which the installer creates as a byte copy of itself.
    Write-Host "Signing the installer..." -ForegroundColor Cyan
    Invoke-CodeSigning -Config $signing -Path $builtExe
}

Copy-Item $builtExe $outputExe -Force

if ($signing -and -not (Test-SignatureTrust -Path $outputExe -Config $signing)) {
    Write-Warning ("The signature does not chain to a trusted root on this machine. That is expected " +
                   "for a self-signed or internal CA certificate, and it means Windows SmartScreen " +
                   "will still warn end users. Only a publicly trusted code signing certificate avoids that.")
}

Write-Host "Done. Installer written to $outputExe" -ForegroundColor Green
