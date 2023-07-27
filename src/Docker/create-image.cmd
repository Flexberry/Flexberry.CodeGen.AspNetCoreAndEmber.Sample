docker build --no-cache -f SQL\Dockerfile.PostgreSql -t supersimplecontactlist/postgre-sql ../SQL

docker build --no-cache -f Dockerfile -t supersimplecontactlist/app ../..

docker push supersimplecontactlist/postgre-sql

docker push supersimplecontactlist/app