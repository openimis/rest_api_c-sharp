FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
WORKDIR /app

COPY /OpenImis.RestApi/*.csproj ./
RUN dotnet restore
ARG BUILD-FLAVOUR=Release
COPY . ./
RUN dotnet publish OpenImis.RestApi/OpenImis.RestApi.csproj -c $BUILD-FLAVOUR -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1
WORKDIR /app
COPY --from=build-env /app/OpenImis.RestApi/out .
ENTRYPOINT ["dotnet", "OpenImis.RestApi.dll"]
