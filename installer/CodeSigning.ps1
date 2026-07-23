<#
.SYNOPSIS
    Authenticode signing helpers, dot-sourced by build-installer.ps1.

.DESCRIPTION
    Wraps signtool.exe from the Windows SDK. Signing is opt-in: when no certificate
    is configured the build still runs and simply produces unsigned binaries, so a
    plain local build needs no certificate at all.

    Two certificate sources are supported:

      Thumbprint  - the certificate lives in the Windows certificate store
                    (CurrentUser\My or LocalMachine\My). Preferred, and the only
                    option for a cert held on a hardware token or HSM, which is
                    how publicly trusted OV/EV code signing certificates must be
                    stored under the CA/Browser Forum rules.

      PFX file    - a .pfx path plus a password from an environment variable.
                    Convenient, but signtool only accepts the password as a
                    command-line argument, which is briefly readable by other
                    processes on the same machine. Prefer the thumbprint on any
                    shared or multi-user build agent.
#>

<#
.SYNOPSIS
    Locates the newest signtool.exe from the installed Windows SDKs.
#>
function Find-SignTool {
    # Honour an explicit override first, for SDK layouts this search doesn't predict.
    if ($env:SIGNTOOL_PATH -and (Test-Path $env:SIGNTOOL_PATH)) { return $env:SIGNTOOL_PATH }

    $existing = Get-Command signtool.exe -ErrorAction SilentlyContinue
    if ($existing) { return $existing.Source }

    $roots = @("${env:ProgramFiles(x86)}\Windows Kits\10\bin", "$env:ProgramFiles\Windows Kits\10\bin") |
        Where-Object { $_ -and (Test-Path $_) }

    # Versioned SDK folders sort as text, so 10.0.9 would beat 10.0.22621; parse the
    # version out and sort numerically instead.
    $candidates = foreach ($root in $roots) {
        Get-ChildItem $root -Directory -ErrorAction SilentlyContinue |
            Where-Object { $_.Name -match '^\d+(\.\d+)+$' } |
            ForEach-Object {
                $tool = Join-Path $_.FullName "x64\signtool.exe"
                if (Test-Path $tool) { [pscustomobject]@{ Version = [version]$_.Name; Path = $tool } }
            }
    }

    $newest = $candidates | Sort-Object Version -Descending | Select-Object -First 1
    if ($newest) { return $newest.Path }

    return $null
}

<#
.SYNOPSIS
    Resolves the signing configuration, or $null when signing is not configured.
#>
function Get-SigningConfig {
    param(
        [string]$Thumbprint,
        [string]$CertificatePath,
        [string]$TimestampUrl
    )

    if (-not $Thumbprint -and -not $CertificatePath) { return $null }

    if ($Thumbprint -and $CertificatePath) {
        throw "Specify either a certificate thumbprint or a .pfx path, not both."
    }

    $signTool = Find-SignTool
    if (-not $signTool) {
        throw "Signing was requested but signtool.exe was not found. Install the Windows SDK " +
              "(Windows 10/11 SDK, 'Windows SDK Signing Tools' component), or set SIGNTOOL_PATH."
    }

    $password = $null
    if ($CertificatePath) {
        if (-not (Test-Path $CertificatePath)) {
            throw "Certificate file not found: $CertificatePath"
        }
        $password = $env:RESHUT_SIGN_PFX_PASSWORD
        if (-not $password) {
            throw "A .pfx was given but RESHUT_SIGN_PFX_PASSWORD is not set."
        }
    }

    return [pscustomobject]@{
        SignTool        = $signTool
        Thumbprint      = $Thumbprint
        CertificatePath = $CertificatePath
        Password        = $password
        TimestampUrl    = $TimestampUrl
    }
}

<#
.SYNOPSIS
    Authenticode-signs one or more files in a single signtool invocation.
#>
function Invoke-CodeSigning {
    param(
        [Parameter(Mandatory)][string[]]$Path,
        [Parameter(Mandatory)]$Config
    )

    $missing = $Path | Where-Object { -not (Test-Path $_) }
    if ($missing) { throw "Cannot sign, file(s) not found: $($missing -join ', ')" }

    # SHA-256 throughout: SHA-1 Authenticode is no longer accepted by Windows.
    # /tr is an RFC 3161 timestamp - without one, every signature stops validating
    # the day the certificate expires, including on copies already distributed.
    $arguments = @('sign', '/fd', 'sha256', '/tr', $Config.TimestampUrl, '/td', 'sha256')

    if ($Config.Thumbprint) {
        $arguments += @('/sha1', $Config.Thumbprint)
    }
    else {
        $arguments += @('/f', $Config.CertificatePath, '/p', $Config.Password)
    }

    $arguments += $Path

    # Public timestamp servers rate-limit and occasionally drop requests, and a failed
    # timestamp fails the whole signature, so give it a few attempts before giving up.
    $maxAttempts = 3
    for ($attempt = 1; $attempt -le $maxAttempts; $attempt++) {
        & $Config.SignTool @arguments 2>&1 | ForEach-Object {
            # Never echo the password back out if signtool quotes the command line.
            if ($Config.Password) { $_ -replace [regex]::Escape($Config.Password), '***' } else { $_ }
        } | Write-Verbose

        if ($LASTEXITCODE -eq 0) {
            foreach ($file in $Path) { Write-Host "  signed  $(Split-Path $file -Leaf)" -ForegroundColor DarkGray }
            return
        }

        if ($attempt -lt $maxAttempts) {
            Write-Warning "signtool failed (exit $LASTEXITCODE), retrying ($attempt/$maxAttempts)..."
            Start-Sleep -Seconds (3 * $attempt)
        }
    }

    throw "signtool failed with exit code $LASTEXITCODE after $maxAttempts attempts. " +
          "Re-run with -Verbose to see its output."
}

<#
.SYNOPSIS
    Reports whether a signature chains to a trusted root on this machine.
#>
function Test-SignatureTrust {
    param(
        [Parameter(Mandatory)][string]$Path,
        [Parameter(Mandatory)]$Config
    )

    & $Config.SignTool verify /pa /q $Path 2>&1 | Write-Verbose
    $trusted = $LASTEXITCODE -eq 0

    # An untrusted chain is a reportable condition, not a build failure. Left as-is, the
    # non-zero code lingers as the script's exit status and CI reads the build as broken.
    $global:LASTEXITCODE = 0
    return $trusted
}
