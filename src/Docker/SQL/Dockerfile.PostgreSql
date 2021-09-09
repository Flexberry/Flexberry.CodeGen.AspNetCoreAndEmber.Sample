FROM flexberry/alt.p8-postgresql

COPY PostgreSql.create.sql /docker/

RUN \
  /docker-cmd.sh& \
  cd /docker/; \
  echo  "\
CREATE DATABASE appdb;\
CREATE USER flexberryuser WITH password 'jhv';\
GRANT ALL privileges ON DATABASE appdb TO flexberryuser;\
" > create.sql;  \
  until psql -U postgres <create.sql; do echo "Wait...";sleep 2; done ; \ 
  until psql -U flexberryuser -d appdb <PostgreSql.create.sql; do echo 'Wait...';sleep 2; done ;\
  /etc/init.d/postgresql stop;  \
  while su -c psql postgres </dev/null >/dev/null 2>&1; do sleep 1; done; echo "postgresql stopped"