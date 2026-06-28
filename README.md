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

## Logging

Logs are written to stderr for compatibility with systemd and container runtimes. Every request is logged with method, path, status code, duration, and a trace correlation ID.

**Log level overrides** (via environment variable):

```bash
# Set the default log level for all categories (including CiCd)
export Logging__LogLevel__Default=Debug

# Or fine-tune per category
export Logging__LogLevel__CiCd=Debug
export Logging__LogLevel__Microsoft__AspNetCore=Information
```

Sensitive query-string parameters (password, secret, token, key, auth) are automatically redacted in request logs.