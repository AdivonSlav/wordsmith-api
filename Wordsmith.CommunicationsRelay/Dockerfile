# Setting up the runtime (Using .NET 6 Runtime on Ubuntu 22.04)
FROM mcr.microsoft.com/dotnet/aspnet:6.0-jammy-amd64 AS base
WORKDIR /app

# Build stage (Using .NET 6 SDK on ubuntu 22.04)
FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy-amd64 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wordsmith.CommunicationsRelay/Wordsmith.CommunicationsRelay.csproj", "Wordsmith.CommunicationsRelay/"]
COPY ["Wordsmith.Utils/Wordsmith.Utils.csproj", "Wordsmith.Utils/"]
COPY ["Wordsmith.Models/Wordsmith.Models.csproj", "Wordsmith.Models/"]
RUN dotnet restore "Wordsmith.CommunicationsRelay/Wordsmith.CommunicationsRelay.csproj"
COPY . .
WORKDIR "/src/Wordsmith.CommunicationsRelay"
RUN dotnet build "Wordsmith.CommunicationsRelay.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Wordsmith.CommunicationsRelay.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wordsmith.CommunicationsRelay.dll"]
