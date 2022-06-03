
FROM node:10 as ember_app

#COPY . /ember-app

WORKDIR /ember-app

COPY ember-app/ .


RUN \
  set -x; \
  yarn global add ember-cli@3.1.4; \
  yarn global add bower;

RUN \
  set -x; \
  yarn install ; \
  bower install  --allow-root;

CMD ls -la

RUN \
  set -x; \
  ember build --environment=docker

FROM mcr.microsoft.com/dotnet/sdk:3.1 as backend

COPY /SuperSimpleContactList .


RUN \
 dotnet restore SuperSimpleContactList.sln;\
 dotnet build SuperSimpleContactList.sln -c Release;\
 dotnet publish ODataBackend/SuperSimpleContactList.ODataBackend.csproj -c Release -o out -f netcoreapp3.1

FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app

COPY --from=backend /out .
COPY --from=ember_app /ember-app/dist .

# Check webapi responses
HEALTHCHECK --interval=60s --timeout=15s --start-period=20s \
  CMD curl -f "http://0.0.0.0/health" || exit

ENTRYPOINT ["dotnet", "SuperSimpleContactList.ODataBackend.dll"]
