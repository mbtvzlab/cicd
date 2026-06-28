# CiCd

## Development

Enter the development shell with nix (optional):

```bash
nix develop
```

Start a docker container with the credentials in `appsettings.json`:

```bash
docker run -d --name cicd-db -p 54321:5432 -e POSTGRES_USER=cicd -e POSTGRES_PASSWORD=cicd123 -e POSTGRES_DB=cicd postgres:latest
```

Run the migrations:

```bash
dotnet ef database update
```

Run the app itself:

```bash
dotnet run
```

## Demo with Docker Compose

A `docker-compose.yml` is included to run the application together with a Postgres database.

```bash
docker compose up --build
```

The app will be available at <http://localhost:8080>. Migrations are applied automatically on startup, and the database data is persisted in a Docker volume.