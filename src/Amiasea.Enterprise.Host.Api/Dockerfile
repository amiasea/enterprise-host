
# syntax=docker/dockerfile:1.7

FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build

WORKDIR /src

# Copy restore inputs first to allow Docker to cache dependency restore.
COPY nuget.config ./nuget.config
COPY src/Amiasea.Enterprise.Host.Api/Amiasea.Enterprise.Host.Api.csproj \
     src/Amiasea.Enterprise.Host.Api/

RUN --mount=type=secret,id=codeartifact_token \
    set -eu; \
    CONFIG=/tmp/nuget.config; \
    trap 'rm -f "$CONFIG"' EXIT; \
    cp /src/nuget.config "$CONFIG"; \
    TOKEN="$(cat /run/secrets/codeartifact_token)"; \
    test -n "$TOKEN"; \
    dotnet nuget update source "amiasea/nuget-enterprise" \
      --username aws \
      --password "$TOKEN" \
      --store-password-in-clear-text \
      --configfile "$CONFIG"; \
    unset TOKEN; \
    dotnet restore \
      src/Amiasea.Enterprise.Host.Api/Amiasea.Enterprise.Host.Api.csproj \
      --configfile "$CONFIG" \
      -p:Configuration=Release

COPY . .

RUN dotnet publish \
    src/Amiasea.Enterprise.Host.Api/Amiasea.Enterprise.Host.Api.csproj \
    --configuration Release \
    --no-restore \
    -p:UseEndpointPackage=true \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:11.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Amiasea.Enterprise.Host.Api.dll"]