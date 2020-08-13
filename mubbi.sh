#!/bin/bash
# Author: felipe-allmeida
# THis helped me a lot, hope it helps you too: https://www.shellcheck.net/
# ----------------------------
Operation="$1"

echo "Operation: $Operation"
if [ "start" = "$Operation" ]; then
docker-compose up -d
elif [ "stop" = "$Operation" ]; then
docker-compose stop
elif [ "delete" = "$Operation" ]; then
docker-compose down -v
elif [ "bootstrap" = "$Operation" ]; then
docker-compose build --no-cache
docker-compose up -d
elif [ "sonar" = "$Operation" ]; then
dotnet dotnet-sonarscanner begin /k:"mubbi" /d:sonar.login="b94109ff1992f3b29d2db57ebf84d32bad408de8" /d:sonar.host.url="http://localhost:9999" /d:sonar.language="cs" /d:sonar.cs.opencover.reportsPaths="**/coverageResults/coverage.opencover.xml"
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat='opencover' /p:CoverletOutput='coverageResults/'
dotnet dotnet-sonarscanner end /d:sonar.login="b94109ff1992f3b29d2db57ebf84d32bad408de8" 
fi