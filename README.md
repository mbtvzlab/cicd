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
