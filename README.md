# Wordsmith API
REST API service for a Flutter eBook reader and marketplace application. It is built with .NET 6.0 and utilizes MySQL and RabbitMQ.

- [Wordsmith API](#wordsmith-api)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Initial Setup](#initial-setup)
    - [Running with Docker](#running-with-docker)
    - [Running directly with the .NET Runtime](#running-directly-with-the-net-runtime)
  - [Documentation and Testing](#documentation-and-testing)
  - [License](#license)

## Getting Started

### Prerequisites
Before using the source code, ensure you have the following installed and configured
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (Includes the ASP.NET Core Runtime)
  
Furthermore, you should have the following services running
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) (v8.0 or newer)
- [RabbitMQ](https://www.rabbitmq.com/) (v3.11 or newer)
- [Docker](https://docs.docker.com/engine/) (if you intend to run this inside of a container)

### Initial Setup
Clone the repository and change into the repo
```bash
git clone https://github.com/AdivonSlav/wordsmith-api && cd wordsmith-api
```

Afterwards, you need to open [compose.yml](compose.yml) and set any required environment variables, particularly the PayPal client and secret. All of the others should already be preconfigured and working with their defaults.

If you are running this outside of a Docker container, check for the env vars in appsettings.json.

For info regarding all the available environment variables, see [.env.example](.env.example).

### Credentials
```bash
# Admin account
Username: orwell47
Password: default$123
PayPal email: orwell47@personal.com
PayPal password: default$123

# Normal user account
Username: john_doe1
Password: default$123
PayPal email: john_doe1@personal.com
PayPal password: default$123

# Seller user acount (will have published books)
Username: jane_doe2
Password: default$123
PayPal email: jane_doe2@personal.com
PayPal password: default$123
```

### Running with Docker
To build the necessary images and run the provided [compose.yml](compose.yml), simply run the following command
```bash
docker compose up
```
The API and Identity Server should be listening on ports 6443 and 7443 respectively. The containers use a self-signed certificate for test purposes to facilitate SSL encryption.

**Note:** Check [compose.yml](compose.yml) to see if you need to set any environment variables explicitly!

### Running directly with the .NET Runtime
Run the following commands to start both the API and the Identity Server. These will restore all dependencies, build and run the applications.
```bash
dotnet run --project Wordsmith.API -c Debug
dotnet run --project Wordsmith.IdentityServer -c Debug
```
The API and Identity Server should be listening on ports 6000/6443 and 7000/7443 respectively.

**Note:** Check the appsettings.json file of each app to see if you need to set any environment variables explicitly!

## Documentation and Testing
OpenAPI documentation is located at *localhost:6443/swagger* by default if running a Debug configuration. 

Some of the projects and/or subfolders will contain separate *README* files that give a general overview and better document what they do.

For testing purposes, it is recommended to use [Postman](https://www.postman.com/) for a more comprehensive experience.

## License
This project is licensed under Apache 2.0. For more information on, read the [LICENSE](LICENSE) file. In short, do whatever you want as this is a university project.
