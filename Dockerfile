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

ENV DB_HOST=Server
ENV DB_NAME=IMIS
ENV DB_USER=IMISuser
ENV DB_PASSWORD=IMISuser@1234


# copy appsettings templates
COPY OpenImis.RestApi/config/appsettings.Production.json.dist /app/tpl
COPY OpenImis.RestApi/config/appsettings.json.dist /app/config
COPY scripts/entrypoint.sh /app/entrypoint.sh


ENTRYPOINT /app/entrypoint.sh
