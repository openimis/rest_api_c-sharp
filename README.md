# openIMIS REST API

The openIMIS REST API is intended to replace the openIMIS Web Services.
It allows the openIMIS mobile phone applications to connect and 
exchange openIMIS related data. 

## Getting Started

These instructions will get you a copy of the project up and running 
on your local machine for development and testing purposes. See 
deployment section for notes on how to deploy the project on a live system.

### Prerequisites

In order to use and develop the openIMIS Web Application on your 
local machine, you first need to install:

* [SQL Server instance](https://github.com/openimis/database_ms_sqlserver)
* [NET Core SDK](https://www.microsoft.com/net/learn/get-started-with-dotnet-tutorial) or Visual Studio with .NET Core SDK

### Installing

To make a copy of this project on your local machine, please clone the repository.

```
git clone https://github.com/openimis/rest_api_c-sharp
```

Restore the NuGet packages needed by the application using VS or [nuget CLI](https://www.nuget.org/downloads).

```
nuget restore
```

Compile the application with VS or with dotnet cli tool

```
dotnet build
```

Before running the application, you need to change the connection string to connect to the 
database in the [appsettings.Development.json.dist](./OpenImis.RestApi/config/appsettings.Development.json.dist) file within [OpenImis.RestApi.config](./OpenImis.RestApi/config) folder. File name should be changed to `appsettings.Development.json` (`.dist` extension removed).

```
"ConnectionStrings":{"IMISDatabase":"Server=Server;Database=IMIS;User ID=User;Password=Password"}
```

Run the application from VS or using dotnet cli tool

```
dotnet run -p OpenImis.RestApi
```

## Deployment

For deployment please read the [installation manual](https://openimis.atlassian.net/wiki/spaces/OP/pages/906985575).

<!--## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 
-->


## User Manual 

The user manual can be read on [docs.openimis.org](http://docs.openimis.org/).

## License

Copyright (c) Swiss Agency for Development and Cooperation (SDC)

This project is licensed under the GNU AGPL v3 License - see the [LICENSE.md](LICENSE.md) file for details.
