# Setting up the runtime (Using .NET6 Runtime on Ubuntu 22.04)
FROM mcr.microsoft.com/dotnet/aspnet:6.0-jammy-amd64 AS base
WORKDIR /app
EXPOSE 443
# Can be overriden in a compose file 
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=https://+443
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/identity_server.crt
ENV ASPNETCORE_Kestrel__Certificates__Default__KeyPath=/https/identity_server.key
ENV WORDSMITH_Connection__IdentityServer__Port=443
ENV WORDSMITH_Logging__NLog__LogLevel=Info

# Build stage (Using .NET6 SDK on Ubuntu 22.04)
FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy-amd64 AS build
WORKDIR /src
COPY ["Wordsmith.IdentityServer/Wordsmith.IdentityServer.csproj", "Wordsmith.IdentityServer/"]
COPY ["Wordsmith.Utils/Wordsmith.Utils.csproj", "Wordsmith.Utils/"]
COPY ["Wordsmith.Models/Wordsmith.Models.csproj", "Wordsmith.Models/"]
RUN dotnet restore "Wordsmith.IdentityServer/Wordsmith.IdentityServer.csproj"
COPY . .
WORKDIR "/src/Wordsmith.IdentityServer"
RUN dotnet build "Wordsmith.IdentityServer.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Wordsmith.IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copies over the self-signed SSL certificate and updates the Ubuntu trusted certificates store
RUN rm -rf /usr/local/share/ca-certificates/aspnet && mkdir -p /usr/local/share/ca-certificates/aspnet
RUN cp ./certs/ca.crt /usr/local/share/ca-certificates/aspnet/ca.crt
RUN update-ca-certificates
RUN mkdir -p /https && cp ./certs/* /https
# The wait-for script ensures that the entrypoint will not be called until the database is ready to accept connections
# Netcat is installed so TCP listening works
RUN apt update && apt install wget netcat -y
RUN wget -qO wait-for.sh https://raw.githubusercontent.com/eficode/wait-for/v2.2.4/wait-for
RUN chmod +x wait-for.sh
CMD ["sh", "-c", "./wait-for.sh $WORDSMITH_Connection__MySQL__Host:$WORDSMITH_Connection__MySQL__Port -- dotnet Wordsmith.IdentityServer.dll"]
