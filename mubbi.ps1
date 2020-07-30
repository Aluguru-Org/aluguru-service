[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)]
    [ValidateNotNull()]
    [ValidateNotNullOrEmpty()]
    [ValidateSet("start", "stop", "delete", "bootstrap", "sonar")]
    [String]
    $Operation
)
begin {
    Write-Host "======================> Mubbi.ps1 started" -ForegroundColor Green
    Write-Host "Operation:" -NoNewline
    Write-Host "$Operation" -ForegroundColor Yellow
}

process {
    Write-Host "======================> Mubbi.ps1 processing" -ForegroundColor Green

    if ($Operation -eq "start") {
        Write-Host "      ___           ___           ___           ___                 " -ForegroundColor DarkRed
        Write-Host "     /\__\         /\__\         /\  \         /\  \          ___   " -ForegroundColor DarkRed
        Write-Host "    /::|  |       /:/  /        /::\  \       /::\  \        /\  \  " -ForegroundColor DarkRed
        Write-Host "   /:|:|  |      /:/  /        /:/\:\  \     /:/\:\  \       \:\  \ " -ForegroundColor DarkRed
        Write-Host "  /:/|:|__|__   /:/  /  ___   /::\~\:\__\   /::\~\:\__\      /::\__\" -ForegroundColor DarkRed
        Write-Host " /:/ |::::\__\ /:/__/  /\__\ /:/\:\ \:|__| /:/\:\ \:|__|  __/:/\/__/" -ForegroundColor DarkRed
        Write-Host " \/__/~~/:/  / \:\  \ /:/  / \:\~\:\/:/  / \:\~\:\/:/  / /\/:/  /   " -ForegroundColor DarkRed
        Write-Host "       /:/  /   \:\  /:/  /   \:\ \::/  /   \:\ \::/  /  \::/__/    " -ForegroundColor DarkRed
        Write-Host "      /:/  /     \:\/:/  /     \:\/:/  /     \:\/:/  /    \:\__\    " -ForegroundColor DarkRed
        Write-Host "     /:/  /       \::/  /       \::/__/       \::/__/      \/__/    " -ForegroundColor DarkRed
        Write-Host "     \/__/         \/__/         ~~            ~~                   " -ForegroundColor DarkRed

        docker-compose up -d
    } elseif ($Operation -eq "stop") {
        docker-compose stop    
    } elseif ($Operation -eq "delete") {
        docker-compose down -v
    } elseif($Operation -eq "sonar") {
        dotnet dotnet-sonarscanner begin /k:"mubbi" /d:sonar.login="684360a5c37971ef20f1e835bb7c1e3281a6a77d" /d:sonar.host.url="http://localhost:9999" /d:sonar.language="cs" /d:sonar.cs.opencover.reportsPaths="**/coverageResults/coverage.opencover.xml"
        dotnet build
        dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat='opencover' /p:CoverletOutput='coverageResults/'
        dotnet dotnet-sonarscanner end /d:sonar.login="684360a5c37971ef20f1e835bb7c1e3281a6a77d"        
    } elseif ($Operation -eq "bootstrap") {
        # Installing necessary dotnet tools
        dotnet tool install dotnet-sonarscanner

        # Building container images
        docker-compose build
    }    
}

end {
    Write-Host "======================> Mubbi.ps1 finished processing" -ForegroundColor Green
}

