# Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./DeliveryDeck_Backend_Final.Auth/DeliveryDeck_Backend_Final.Auth.csproj" --disable-parallel
RUN dotnet publish "./DeliveryDeck_Backend_Final.Auth/DeliveryDeck_Backend_Final.Auth.csproj" -c release -o /app --no-restore

# Serve
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS serve
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "DeliveryDeck_Backend_Final.Auth.dll"]