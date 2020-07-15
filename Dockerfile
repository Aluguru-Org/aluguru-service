FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY . /app
WORKDIR /app
RUN dotnet tool install --local dotnet-ef
RUN dotnet restore ./src/Mubbi.Marketplace.API/Mubbi.Marketplace.API.csproj
RUN dotnet build ./src/Mubbi.Marketplace.API/Mubbi.Marketplace.API.csproj -c Release --no-restore
#RUN dotnet tool run dotnet-ef database update --project ./src/Mubbi.Marketplace.API/Mubbi.Marketplace.API.csproj -v
EXPOSE 5000/tcp

WORKDIR /app/src/Mubbi.Marketplace.API
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
#ENTRYPOINT ["dotnet", "Mubbi.Marketplace.API.dll", "--urls http://*:5000"]

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app .

#dotnet Mubbi.Marketplace.API.dll --urls http://*:5000