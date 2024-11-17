# panpipe-backend
Backend part of project Panpipe

## How to run
`docker compose up -d --build` for usual run. Then navigate to `localhost:8080/swagger`.

`docker compose --profile debug-sql up --build` to be able to debug sql queries with pgadmin. Navigate to `localhost:8081` for pgadmin. Credentials for last are stored in `docker-compose.yml`. Credentials for db to connect in pgadmin are also stored in `docker-compose.yml` + `host=db`.
