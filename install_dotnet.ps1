Set-ExecutionPolicy RemoteSigned -Scope CurrentUser


$currentPrincipal = New-Object Security.Principal.WindowsPrincipal(
                    [Security.Principal.WindowsIdentity]::GetCurrent()
                    )
$isAdmin = $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if ($isAdmin) {
    
    $dotnetsdklist = "*.NET SDK*"

    $installed = Get-Package | Where-Object -Property Name -like "*.NET SDK*" | Select-Object -Property Version

    if (!$installed)
    {
        echo "No any of SDKS is installed on this system."
        echo "Installing Latest Dotnet SDK ... (8 on this post)"
        winget install -e --id Microsoft.DotNet.SDK.Preview
    }
    else
    {
        echo "Checking the version of current dotnet runtime ..."
        $current_dotnet_version = dotnet --version
        $major_version = [int] $current_dotnet_version.split(".")[0]

        if ($major_version -ne 8)
        {
            echo "Installed $major_version. This project requires latest version ..."
            echo "Installing SDK 8 ..."
            winget install -e --id Microsoft.DotNet.SDK.Preview
        }
        else
        {
            echo "SDK 8 already installed on this system"
        }
        
    }
} else {
    Write-Host "PowerShell is not running as admin."
}