FROM node:10 as ember_app

COPY /src/ember-app /src/ember-app

RUN \
  set -x; \
  cd /src/ember-app/; \
  yarn global add ember-cli@3.1.4; \
  yarn global add bower;

RUN \
  set -x; \
  cd /src/ember-app/; \
  yarn install ; \
  bower install  --allow-root;

RUN \
  set -x; \
  cd /src/ember-app/; \
  ember build --environment=docker

FROM mcr.microsoft.com/dotnet/sdk:latest as backend

COPY /src/SuperSimpleContactList /src/SuperSimpleContactList

WORKDIR /src/SuperSimpleContactList

RUN \
 cd /src/SuperSimpleContactList ;\
 dotnet restore SuperSimpleContactList.sln -f netcoreapp3.1;\
 dotnet publish SuperSimpleContactList.sln -c Release -o out -f netcoreapp3.1

FROM flexberry/mono-xsp:latest

MAINTAINER mail@flexberry.net

COPY --from=backend /src/SuperSimpleContactList/ODataBackend /app
COPY --from=ember_app /src/ember-app/dist /app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=backend /src/SuperSimpleContactList/out ./
COPY --from=ember_app /src/ember-app/dist ./
ENTRYPOINT ["dotnet", "aspnetapp.dll"]
