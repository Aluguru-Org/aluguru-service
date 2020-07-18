FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY . /app
WORKDIR /app
RUN dotnet tool install --local dotnet-ef
RUN dotnet restore ./src/Mubbi.Marketplace.API/Mubbi.Marketplace.API.csproj
RUN dotnet build ./src/Mubbi.Marketplace.API/Mubbi.Marketplace.API.csproj -c Release --no-restore
EXPOSE 5000/tcp

WORKDIR /app/src/Mubbi.Marketplace.API
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
