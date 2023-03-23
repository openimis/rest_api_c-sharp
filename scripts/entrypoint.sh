#!/bin/bash

envsubst  '${DB_HOST},${DB_USER},${DB_PASSWORD},${DB_NAME}' < /app/tpl/appsettings.Production.json.dist > /app/config/appsettings.Production.json

dotnet OpenImis.RestApi.dll