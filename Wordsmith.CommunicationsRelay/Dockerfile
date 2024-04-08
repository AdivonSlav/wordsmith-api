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
# The wait-for script ensures that the entrypoint will not be called until the RabbitMQ server is ready to accept connections
# Netcat is installed so TCP listening works
RUN apt update && apt install wget netcat -y
RUN wget -qO wait-for.sh https://raw.githubusercontent.com/eficode/wait-for/v2.2.4/wait-for
RUN chmod +x wait-for.sh
CMD ["sh", "-c", "./wait-for.sh $WORDSMITH_Connection__RabbitMQ__Host:5672 -- dotnet Wordsmith.CommunicationsRelay.dll"]
