#!/bin/sh
#set -x

cd /app

dotnet SuperSimpleContactList.Service.dll

# Пример запуска основного бэкенда с фоновым сервисом
# dotnet SuperSimpleContactList.Service.dll &
# dotnet SuperSimpleContactList.ODataBackend.dll