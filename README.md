# KeyRails.BankingApi

KeyRails.BankingApi is the primary solution in this repository. It includes the HTTP API, application layer, domain layer, infrastructure layer, and background worker service.

## Solution Layout

- `src/KeyRails.BankingApi.Api` hosts the API surface.
- `src/KeyRails.BankingApi.Application` contains application logic.
- `src/KeyRails.BankingApi.Domain` contains domain entities and rules.
- `src/KeyRails.BankingApi.Infrastructure` contains persistence and integration concerns.
- `src/KeyRails.BankingApi.WorkerService` hosts background processing.

## Build

```bash
dotnet build KeyRails.BankingApi.sln
```

## Run

Run the API:

```bash
dotnet run --project src/KeyRails.BankingApi.Api/KeyRails.BankingApi.Api.csproj
```

Run the worker service:

```bash
dotnet run --project src/KeyRails.BankingApi.WorkerService/KeyRails.BankingApi.WorkerService.csproj
```

## Test

```bash
dotnet test KeyRails.BankingApi.sln
```

## Database Migrations

```bash
dotnet ef migrations add <MigrationName> -s src/KeyRails.BankingApi.Api -p src/KeyRails.BankingApi.Infrastructure
dotnet ef database update -s src/KeyRails.BankingApi.Api -p src/KeyRails.BankingApi.Infrastructure
```
