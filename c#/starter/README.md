# FlightDelay Workshop Starter Scaffold

This is a minimal ASP.NET Core Web API starter project for the FlightDelay workshop. It includes:

- `FlightDelay.Api`: A basic Web API project with minimal APIs and Swagger.
- `FlightDelay.Tests`: An xUnit test project referencing the API.

## Quick Start

1. Open `FlightDelayWorkshop.sln` in Visual Studio or VS Code
2. Run `dotnet build` to ensure it compiles
3. Run `dotnet test` to run the tests
4. Follow the workshop instructions in `../content/`

## Workshop Progress Checklist

Follow the instructions in the `../content/` folder. Each instruction file has its own detailed checklist:

- **Step 1**: [Domain Model & Data Layer](../content/1-create-model-data.md) - Design entities, add EF Core, create repository, migrations, and CSV importer
- **Step 2**: [API Layer](../content/2-create-api.md) - Create endpoints, add validation, security, middleware, and tests
- **Step 3**: [Integration & Verification](../content/3-create-integration-verification.md) _(optional)_ - Build console client or API testing tools
- **Step 4**: [Advanced Exercises](../content/4-advanced-backend-exercises.md) _(optional)_ - Microservices, event sourcing, security, performance, observability

## Tips for Using GitHub Copilot

- **Open related files**: Keep your Flight entity open when writing FlightRepository to help Copilot infer context
- **Start specific, then expand**: Begin with "Create a Flight entity with Id, Airline, Origin properties" before asking for full implementations
- **Iterate on suggestions**: Accept a suggestion, then refine with follow-up prompts like "Add validation to ensure ScheduledDeparture is required"
- **Test Copilot output**: Always write tests for generated code to verify behavior

## Getting Unstuck

### Domain Model Issues
- **"I don't know what entities to create"**: Look at the CSV columns in `../data/flights.csv` - each major concept (Flight, Airline, Airport) should be an entity
- **"Value object vs entity?"**: If it has no identity and is defined by its attributes (like Coordinates), make it a value object (immutable record)
- **Example Copilot prompt**: "Create an immutable Coordinates value object with Latitude and Longitude validation in C# using record types"

### EF Core Configuration
- **"Owned types aren't working"**: Use `OwnsOne()` in OnModelCreating to map value objects as owned types so they flatten into the parent table
- **Example Copilot prompt**: "Show me how to configure FlightIdentifier as an owned type in EF Core using Fluent API"
- **Migration errors**: Run `dotnet ef migrations add InitialCreate --project FlightDelay.Api` from the solution folder

### CSV Import
- **"CSV import is slow"**: Batch inserts using `AddRange()` and call `SaveChangesAsync()` every 500-1000 rows
- **Example Copilot prompt**: "Write a method to read CSV using CsvHelper and batch upsert into EF Core every 500 rows"

### Common Commands
```bash
# Add EF Core packages
dotnet add FlightDelay.Api/FlightDelay.Api.csproj package Microsoft.EntityFrameworkCore
dotnet add FlightDelay.Api/FlightDelay.Api.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add FlightDelay.Api/FlightDelay.Api.csproj package Microsoft.EntityFrameworkCore.Sqlite

# Create migration
dotnet ef migrations add InitialCreate --project FlightDelay.Api

# Update database
dotnet ef database update --project FlightDelay.Api

# Build and test
dotnet build
dotnet test
```

This scaffold is for quick setup; advanced users can start from scratch.