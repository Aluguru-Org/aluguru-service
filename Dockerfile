FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY . /app
WORKDIR /app

# Copy csproj and restore as distinct layers
RUN dotnet restore ./src/Mubbi.Marketplace.API/Mubbi.Marketplace.API.csproj

# Copy everything else and build
RUN dotnet publish ./src/Mubbi.Marketplace.API/Mubbi.Marketplace.API.csproj -c Release --no-restore -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app/src/Mubbi.Marketplace.API
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Mubbi.Marketplace.API.dll"]

#HEALTHCHECK --interval=5m --timeout=3s \
  #CMD curl -f http://localhost/ || exit 1