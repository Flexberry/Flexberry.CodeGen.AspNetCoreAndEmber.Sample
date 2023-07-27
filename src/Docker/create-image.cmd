docker build --no-cache -f SQL\Dockerfile.PostgreSql -t supersimplecontactlist/postgre-sql ../SQL

docker build --no-cache -f Dockerfile -t supersimplecontactlist/app ../..

docker build --no-cache -f Dockerfile.WebApi -t supersimplecontactlist/web-api ../..
