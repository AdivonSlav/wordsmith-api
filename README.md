# Wordsmith API
REST API service for a Flutter eBook reader and marketplace application. It is built with ASP.NET Core 6.0 and utilizes MySQL and RabbitMQ.

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Initial Setup](#initial-setup)
  - [Configuration Settings](#configuration-settings)
  - [Building and Running](#building-and-running)
- [Dockerization](#dockerization)
  - [Dockerfiles](#dockerfiles)
  - [Docker Compose](#docker-compose)
- [Documentation and Testing](#documentation-and-testing)
- [License](#license)

## Getting Started

### Prerequisites
Before using the source code, ensure you have the following installed and configured
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (Includes the ASP.NET Core Runtime)
- Any IDE such as [Visual Studio](https://visualstudio.microsoft.com/) or [Jetbrains Rider](https://www.jetbrains.com/rider/). A text editor with .NET integration will also work
  
Furthermore, you should have the following services running
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) (v8.0 or newer)
- [RabbitMQ](https://www.rabbitmq.com/) (v3.11 or newer)

### Initial Setup
Clone the repository and change into the solution folder
```bash
git clone https://github.com/AdivonSlav/wordsmith-api && cd wordsmith-api
```
Afterwards, you will have to edit the *appsettings.json* files found in `Wordsmith.API` and `Wordsmith.IdentityServer`, which are the startup projects of the solution. These contain environment variables and settings, some of which are already set to their default values. For development purposes, it is recommended to create an *appsettings.Development.json* file in the same directory and change any values you want. The .NET Runtime will automatically pick up this file and override settings with the same name if running within a development context (Debug configuration).

### Configuration Settings
The following represent the environment variables supported by the application. They can be set through the respective *appsettings.json* file or as actual environment variables.

<table>
  <tr>
    <th>Environment variables</th>
    <th>Description</th>
  </tr>
  <tr>
    <td>
      <li>WORDSMITH_IdentityServer__Secrets__User</li>
      <li>WORDSMITH_IdentityServer__Secrets__Admin</li>
    </td>
    <td>
      These are used to sign and validate tokens issued by the Identity Server. Should be set to long and secure strings
    </td>
  </tr>
  <tr>
    <td>
      <li>WORDSMITH_Connection__MySQL__Host</li>
      <li>WORDSMITH_Connection__MySQL__Port</li>
      <li>WORDSMITH_Connection__MySQL__User</li>
      <li>WORDSMITH_Connection__MySQL__Password</li>
      <li>WORDSMITH_Connection__MySQL__Database</li>
    </td>
    <td>
      Used to create the connection string for connecting to a MySQL Server instance. Set the values that apply to your instance.
    </td>
  </tr>
  <tr>
    <td>
      <li>WORDSMITH_Connection__RabbitMQ__Host</li>
      <li>WORDSMITH_Connection__RabbitMQ__User</li>
      <li>WORDSMITH_Connection__RabbitMQ__Password</li>
    </td>
    <td>
      Used to connect to the RabbitMQ message broker. Set the values that apply to your instance.
    </td>
  </tr>
  <tr>
    <td>
      <ul>
        <li>WORDSMITH_Connection__IdentityServer__Host</li>
        <li>WORDSMITH_Connection__IdentityServer__Port</li>
      </ul>
    </td>
    <td>
      Tells the API what the hostname and listen port of the Identity Server is. <b>Defaults to localhost and 7443 respectively.</b>
    </td>
  </tr>
  <tr>
    <td>
      <li>WORDSMITH_ImageSettings__AllowedSize</li>
    </td>
    <td>
      The maximum allowed size in bytes for an image to be uploaded with the API. <b>Defaults to 5242880 (5MB).</b>
    </td>
  </tr>
  <tr>
    <td>
      <li>WORDSMITH_ImageSettings__AllowedFormats</li>
    </td>
    <td>
      Comma-separated list of allowed image formats for upload. <b>Defaults to png,jpeg</b>
    </td>
  </tr>
  <tr>
    <td>
      <li>WORDSMITH_Logging__NLog__LogLevel</li>
    </td>
    <td>
      Minimum log level used by the Wordsmith Logger wrapper class. <b>Defaults to Debug.</b>
    </td>
  </tr>
</table>
<hr>

### Building and Running
Running the following command will restore any dependencies and build the solution with the provided configuration. Alternatively, build through your IDE of choice
```bash
dotnet build -c Debug
```
The solution can be ran from an IDE or manually using `dotnet run`. Either way, Wordsmith.API and Wordsmith.IdentityServer must be started together. 

If using an IDE, set both projects as startup projects with your configuration of choice.

If running through a terminal, run both projects with the following commands (for ease of use, maintain two separate terminal tabs). Replace with your configuration of choice
```bash
dotnet run --project Wordsmith.API -c Debug
dotnet run --project Wordsmith.IdentityServer -c Debug
```
The API and Identity Server, by default, listen on ports 6000/6443 and 7000/7443 respectively. This can be changed in the *launchSettings.json* file of each project.

## Dockerization

### Dockerfiles
The startup projects Wordsmith.API and Wordsmith.IdentityServer have Dockerfiles configured and ready to use in each project root. Both images are built and ran on the Ubuntu 22.04 base image with the Release configuration by default.

In a Docker setting, only the HTTPS port is enabled and set to 443 and a self-signed testing certificate is provided. The CA certificate is added to the trusted Ubuntu store the build and is used to sign the server certificates used by both containers at runtime. This approach is meant for purely testing purposes and a different certificate approach should be taken in an actual production environment.

Both container entrypoints are ran with the [wait-for](https://github.com/eficode/wait-for) script. This ensures that neither container will call `dotnet run` until a connection can be made to the MySQL Server container.

### Docker Compose
The solution includes a [docker-compose.yml](docker-compose.yml) file which can be used to start all the necessary services automatically, with configured volumes and environment variables. It is recommended to review the compose file and change any environment variables you might want.

For ease-of-use, the solution provides a script for both [Windows](docker_build_and_run.bat) and [Linux/macOS](docker_build_and_run.sh) for building the images and running the compose file automatically.

If running on Windows, simply execute *docker_build_and_run.bat*

If running Linux or MacOS, run the following commands
```bash
chmod u+x docker_build_and_run.sh
./docker_build_and_run.sh
```

Alternatively, images can be built separately by calling the commands outlined in the script manually and then running `docker compose up -d` in the solution root.

## Documentation and Testing
OpenAPI documentation is located at *localhost:6443/swagger* by default if running a Debug configuration. 

Some of the projects and/or subfolders will contain separate *README* files that give a general overview and better document what they do.

For testing purposes, it is recommended to use [Postman](https://www.postman.com/) for a more comprehensive experience.

## License
This project is licensed under Apache 2.0. For more information on, read the [LICENSE](LICENSE) file. In short, do whatever you want as this is a university project.