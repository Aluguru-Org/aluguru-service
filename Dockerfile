FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY . /app
WORKDIR /app

# Copy csproj and restore as distinct layers
RUN dotnet restore ./src/Aluguru.Marketplace.API/Aluguru.Marketplace.API.csproj

# Copy everything else and build
RUN dotnet publish ./src/Aluguru.Marketplace.API/Aluguru.Marketplace.API.csproj -c Release --no-restore -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic
WORKDIR /app/src/Aluguru.Marketplace.API
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Aluguru.Marketplace.API.dll"]

#HEALTHCHECK --interval=5m --timeout=3s \
  #CMD curl -f http://localhost/ || exit 1