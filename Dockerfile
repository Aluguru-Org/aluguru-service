FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY . /app
WORKDIR /app

# Copy csproj and restore as distinct layers
RUN dotnet restore ./src/Aluguru.Marketplace.API/Aluguru.Marketplace.API.csproj

# Copy everything else and build
RUN dotnet publish ./src/Aluguru.Marketplace.API/Aluguru.Marketplace.API.csproj -c Release --no-restore -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic

# Defining env variables
ENV ConnectionStrings:DefaultConnection=Server=db;Database=AluguruDb-Development;User=sa;Password=sqWkMD4k
ENV AzureStorageSettings:ConnectionString=DefaultEndpointsProtocol=https;AccountName=spdevblobstorage;AccountKey=/V1yzrxm5Be25yWQW9PLe5tK1MG5UXqaG2l7x9xQFVTp5E1+QLuuzCGXOHaj+PoGpOAOzuDRvRzqf0L0g08rzQ==;EndpointSuffix=core.windows.net
ENV JwtSettings:SecretKey=$!z@pgYN^Z3SZGGM@ek&M5UdrbMRs2v-
ENV SecuritySettings:SecretKey=GQ77CCT6Y6THSQQM
ENV GoogleSettings:ApiKey=AIzaSyAO6UrHk4rAfHbEhIyX2A9u6hwxKAWf_cE
ENV MailingSettings:ApiKey=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
ENV IuguSettings:Token=9f49ac361bc7514b01b74f24b5f1701a

WORKDIR /app/src/Aluguru.Marketplace.API
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Aluguru.Marketplace.API.dll"]

#HEALTHCHECK --interval=5m --timeout=3s \
  #CMD curl -f http://localhost/ || exit 1