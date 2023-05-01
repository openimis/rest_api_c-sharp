FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
WORKDIR /app

COPY /OpenImis.RestApi/*.csproj ./
RUN dotnet restore
ARG BUILD-FLAVOUR=Release
COPY . ./
RUN dotnet publish OpenImis.RestApi/OpenImis.RestApi.csproj -c $BUILD-FLAVOUR -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1
WORKDIR /app


ENV DB_HOST=Server
ENV DB_NAME=IMIS
ENV DB_USER=IMISuser
ENV DB_PASSWORD=IMISuser@1234

# copy appsettings templates
COPY ./OpenImis.RestApi/config/appsettings.Production.json.dist /app/tpl/
COPY ./OpenImis.RestApi/config/appsettings.json /app/config/
COPY ./scripts/entrypoint.sh /app/
RUN chmod a+x /app/entrypoint.sh
COPY --from=build-env /app/OpenImis.RestApi/out .
RUN echo 'deb http://archive.debian.org/debian/ stretch main' > /etc/apt/sources.list \
    && echo 'deb http://archive.debian.org/debian-security/ stretch/updates main' >> /etc/apt/sources.list \
    && apt-get -o Acquire::Check-Valid-Until=false update \
    && apt-get install gettext -y \
    && rm -rf /var/lib/apt/lists/*

ENTRYPOINT /app/entrypoint.sh
