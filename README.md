# CodeGenSample.AspNetCorePlusEmber

Тестовый стенд сгенерированного приложения.
Включает в себя следующие компоненты:

1) Ember frontend (верия 3)
2) БД Postgres
3) OdataBackend (NET7) - Odata-бэкенд приложения.
4) WebApi (NET7) - пример Web-api приложения. Для демонстрации содержит пример тестового контроллера.
5) Service (NET7) - пример фонового сервиса. Сделан на основе Microsoft.Extensions.Hosting.BackgroundService
7) ConsoleApp (NET7) - пример простого консольного приложения

Приложения работают в виде докер-контейнеров:

/src/Docker/create-image.cmd - собрать образы.
src/Docker/start.cmd - запустить контейнеры
src/Docker/stop.cmd - остановить контейнеры

