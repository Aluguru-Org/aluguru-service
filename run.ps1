[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)]
    [ValidateNotNull()]
    [ValidateNotNullOrEmpty()]
    [ValidateSet("start", "stop", "delete", "bootstrap", "prune", "sonar", "deploy-dockerhub", "add-migration")]
    [String]
    $Operation,
    [Parameter(Mandatory=$false)]
    [String]
    $OperationArg1
)
begin {
    Write-Host "======================> Aluguru.ps1 started" -ForegroundColor Green
    Write-Host "Operation:" -NoNewline
    Write-Host "$Operation" -ForegroundColor Yellow    
}

process {
    Write-Host "======================> Aluguru.ps1 processing" -ForegroundColor Green

    if ($Operation -eq "start") {
        docker-compose up -d
    } elseif ($Operation -eq "stop") {
        docker-compose stop
    } elseif ($Operation -eq "delete") {
        docker-compose down -v
    } elseif ($Operation -eq "prune") {
        docker system prune -af
    }     
    elseif ($Operation -eq "deploy-dockerhub") {
        docker build . -t felipeallmeidadev/aluguru-service:$OperationArg1
        docker build . -t felipeallmeidadev/aluguru-service:latest

        docker push felipeallmeidadev/aluguru-service:$OperationArg1
        docker push felipeallmeidadev/aluguru-service:latest
    } elseif($Operation -eq "sonar") {
        dotnet dotnet-sonarscanner begin /k:"aluguru" /d:sonar.login="684360a5c37971ef20f1e835bb7c1e3281a6a77d" /d:sonar.host.url="http://localhost:9999" /d:sonar.language="cs" /d:sonar.cs.opencover.reportsPaths="**/coverageResults/coverage.opencover.xml"
        dotnet build
        dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat='opencover' /p:CoverletOutput='coverageResults/'
        dotnet dotnet-sonarscanner end /d:sonar.login="684360a5c37971ef20f1e835bb7c1e3281a6a77d"        
    } elseif ($Operation -eq "bootstrap") {
        # Installing necessary dotnet tools
        # dotnet tool install dotnet-sonarscanner

        # Building container images
        # docker-compose build
        docker-compose up -d --build
    } elseif($Operation -eq "add-migration") {
        dotnet ef migrations add "$OperationArg1" --project src/Aluguru.Marketplace.Data/Aluguru.Marketplace.Data.csproj --startup-project src/Aluguru.Marketplace.API/Aluguru.Marketplace.API.csproj -v
	}
}

end {
    Write-Host "======================> Aluguru.ps1 finished processing" -ForegroundColor Green
}

