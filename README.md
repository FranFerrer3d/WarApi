# WarApi

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/)

## Configuration

Set the following environment variables before running the API:

- `DATABASE_URL` &ndash; connection string for your PostgreSQL instance.
- `JWT_SECRET` &ndash; secret key used to sign JWT tokens.

Example connection string:

```bash
export DATABASE_URL="Host=localhost;Port=5432;Username=postgres;Password=pass;Database=war_api"
export JWT_SECRET="your-secret-key"
```

## Running the API

1. Restore dependencies:
   ```bash
   dotnet restore
   ```
2. Apply database migrations (optional &ndash; they are also applied on startup):
   ```bash
   dotnet ef database update
   ```
3. Start the application:
   ```bash
   dotnet run --project WarApi.csproj
   ```

## Running tests

Execute the unit tests with:

```bash
dotnet test
```

## Docker

Build the Docker image using the provided `Dockerfile`:

```bash
docker build -t warapi .
```

The resulting image runs the API using .NET 8.
