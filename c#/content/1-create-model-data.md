# 1 - Create model and data layer (Advanced)

## Progress Checklist

Track your progress through this step:

- [ ] Create `Domain/Entities/` folder with Flight, Airline, Airport, DelayEvent
- [ ] Create `Domain/ValueObjects/` folder with FlightIdentifier, Coordinates
- [ ] Create `Domain/Enums/` folder with FlightStatus
- [ ] Add validation and invariants to entities
- [ ] Add EF Core packages (EntityFrameworkCore, EntityFrameworkCore.Design, EntityFrameworkCore.Sqlite)
- [ ] Create `Persistence/` folder with FlightDelayDbContext
- [ ] Configure owned types and indexes using Fluent API
- [ ] Define IFlightRepository interface and EfCoreFlightRepository implementation
- [ ] Add initial migration
- [ ] Create CSV importer service for `../../data/flights.csv`
- [ ] Write unit tests for entity invariants
- [ ] Write integration tests for migrations and importer
- [ ] (Optional) Implement at least 2 advanced extensions (optimistic concurrency, CQRS, event sourcing, DDD aggregates, domain services)

---

Goal
Design a resilient domain model and data access layer for flight delay data. Aim for clear boundaries between domain, persistence, and API models. Consider DDD patterns where appropriate.

Tasks
1. Domain model
   - Design entities: Flight, Airline, Airport, FlightStatus, DelayEvent. Include value objects where helpful (e.g., Coordinates, FlightIdentifier).
   - Provide invariants: e.g., ScheduledDeparture must be before ActualDeparture when ActualDeparture is present. Model these checks in constructors or factory methods.

   **Copilot prompt examples:**
   ```
   Create a FlightIdentifier value object using C# record with AirlineCode and FlightNumber properties. Add validation in the constructor to ensure both are non-empty and normalize them to uppercase.
   ```
   
   ```
   Implement a Flight entity class with properties: Id (int), Identifier (FlightIdentifier), ScheduledDeparture (DateTime), ActualDeparture (DateTime?), DelayMinutes (int). Add a RecordActualDeparture method that validates ActualDeparture is not before ScheduledDeparture.
   ```
   
   ```
   Create a Coordinates value object record with Latitude and Longitude. Add constructor validation to ensure Latitude is between -90 and 90, and Longitude is between -180 and 180.
   ```

2. Persistence
   - Add EF Core with a DbContext (FlightDelayDbContext). Configure fluent mappings to enforce constraints and indexes (e.g., index on Airline+FlightNumber+Date).
   - Implement repository interfaces (IFlightRepository) and at least one concrete implementation using EF Core.

   **Copilot prompt examples:**
   ```
   Create a FlightDelayDbContext class inheriting from DbContext with DbSet<Flight>, DbSet<Airline>, and DbSet<Airport>. Add OnModelCreating method with Fluent API configuration.
   ```
   
   ```
   Configure FlightIdentifier as an owned type in EF Core using OwnsOne() so it flattens into the Flight table. Map properties with column names AirlineCode and FlightNumber.
   **Copilot prompt examples:**
   ```
   Create a FlightDataSeeder class with a SeedAsync method that checks if data exists using Any() and only seeds if the database is empty. Make it idempotent.
   ```
   
   ```
   Write a method to read CSV file using CsvHelper library. Map CSV columns (Year, Month, DayofMonth, Carrier, OriginAirportID, DestAirportID, DepDelay) to Flight entity properties.
   ```
   
   ```
   Implement a batch upsert method that reads flights.csv, groups records into batches of 500, and calls AddRange() followed by SaveChangesAsync() for each batch to improve performance.
   ```
   
   ```
   Add a check to the CSV importer: if a flight with the same airline, flight number, and date already exists, update it instead of creating a duplicate.
   ```
   ```
   Add a composite index on Flight entity for AirlineId and ScheduledDeparture using HasIndex() in OnModelCreating to optimize queries.
   ```
   
   ```
   Create an IFlightRepository interface with methods: GetByIdAsync, GetByFlightNumberAsync, AddAsync, UpdateAsync. Include async Task return types.
   ```

3. Migrations and sample data
   - Add migrations and a seed strategy that can run in development (ensure seeding is idempotent).
   - Use a lightweight dataset (from ../../data/flights.csv in the repo) — write a small importer that reads CSV and upserts flights. As a backup, use the sample SQL script in ../data/flights-sample.sql to populate the DB if the importer takes too long.

   Copilot prompt example:
   "Write a console-class service that reads flights.csv and upserts rows into the database using EF Core, batching every 500 rows." 

Advanced extensions (choose at least two)
- Implement optimistic concurrency for updates and demonstrate conflict resolution.
- Add a CQRS split: write models for commands/queries and an in-memory command handler for updates.
- Sketch an event-sourced append-only store for DelayEvent and a projector that materializes Flight projections.
- Implement DDD aggregates: Make Flight an aggregate root with DelayEvent as domain events, using MediatR for publishing.
- Add domain services for complex business logic, e.g., a DelayCalculationService that computes delays based on rules.

Tests and validation
- Unit tests: entity invariants, repository behavior using in-memory database or Sqlite in-memory with real EF Core behavior.
- Integration: run migrations against a test database and import a small sample dataset, assert counts.

Checkpoint
- Domain types created and validated via unit tests.
- DbContext, migrations, and at least one repository implementation exist.

Deliverable
- A `Domain/`, `Persistence/`, and `Services/` folder structure in the API project, migrations, and seed/import tooling.

---

## Getting Unstuck

### Problem: "I don't know what properties Flight should have"
**Solution**: Look at `../../data/flights.csv` - the columns show the raw data structure:
- `Carrier` → map to Airline entity (AirlineId foreign key)
- `OriginAirportID`, `DestAirportID` → map to Airport entities
- `CRSDepTime`, `CRSArrTime` → scheduled times
- `DepDelay`, `ArrDelay` → delay calculations
- `Year`, `Month`, `DayofMonth` → combine into DateTime for ScheduledDeparture

**Copilot prompt**:
```
Based on CSV columns Year, Month, DayofMonth, Carrier, OriginAirportID, DestAirportID, CRSDepTime, DepDelay, create a Flight entity class in C# with appropriate properties and types.
```

### Problem: "Value objects aren't saving to the database"
**Solution**: Value objects must be configured as **owned types** in EF Core using Fluent API.

**Copilot prompt**:
```
In OnModelCreating, configure FlightIdentifier as an owned type using modelBuilder.Entity<Flight>().OwnsOne(f => f.Identifier). Show complete example.
```

**Example code**:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Flight>().OwnsOne(f => f.Identifier, id =>
    {
        id.Property(i => i.AirlineCode).HasColumnName("AirlineCode");
        id.Property(i => i.FlightNumber).HasColumnName("FlightNumber");
    });
}
```

### Problem: "Migration commands aren't working"
**Common issues**:
1. **Missing EF Core tools**: Install globally with `dotnet tool install --global dotnet-ef`
2. **Wrong directory**: Run commands from the solution directory (where `.sln` file is)
3. **Missing Design package**: Add `Microsoft.EntityFrameworkCore.Design` to the API project

**Commands**:
```bash
# From starter/ directory:
dotnet ef migrations add InitialCreate --project FlightDelay.Api
dotnet ef database update --project FlightDelay.Api
```

### Problem: "CSV import is too slow (271k+ rows)"
**Solutions**:
1. **Batch inserts**: Use `AddRange()` with 500-1000 items, then `SaveChangesAsync()`
2. **Disable tracking**: Use `ChangeTracker.AutoDetectChangesEnabled = false` during bulk operations
3. **Skip duplicates**: Check if records exist before inserting (or use the SQL script as backup)

**Copilot prompt**:
```
Optimize this CSV import code to batch 1000 records at a time and disable change tracking during bulk insert using EF Core.
```

### Problem: "Tests are failing on invariants"
**Debug steps**:
1. Check constructor validation logic - are you throwing the right exceptions?
2. Test with xUnit `Assert.Throws<ArgumentException>()` for expected failures
3. Use Arrange-Act-Assert pattern

**Copilot prompt**:
```
Write an xUnit test that verifies Flight.RecordActualDeparture throws InvalidOperationException when departure time is before scheduled time.
```

**Example test**:
```csharp
[Fact]
public void RecordActualDeparture_ThrowsWhenBeforeScheduled()
{
    var flight = new Flight(
        new FlightIdentifier("AA", "100"), 
        new DateTime(2024, 1, 1, 10, 0, 0)
    );
    
    var ex = Assert.Throws<InvalidOperationException>(() => 
        flight.RecordActualDeparture(new DateTime(2024, 1, 1, 9, 0, 0))
    );
}
```

### Problem: "I can't decide between entity and value object"
**Rule of thumb**:
- **Entity**: Has identity (Id), lifecycle, mutable → Flight, Airline, Airport
- **Value Object**: No identity, immutable, defined by attributes → FlightIdentifier, Coordinates

**Examples**:
- `FlightIdentifier` is a value object because "AA100" is "AA100" anywhere - no unique ID needed
- `Flight` is an entity because each flight instance has a unique Id and lifecycle (scheduled → departed → landed)

**Copilot prompt**:
```
Explain when to use an entity vs value object in DDD. Give examples for a flight domain model.
```

### Problem: "Repository pattern feels like overkill"
**Why use it**:
- Decouples domain from persistence technology (can swap EF Core for Dapper later)
- Makes testing easier (mock IFlightRepository instead of DbContext)
- Enforces aggregate boundaries

**Copilot prompt**:
```
Create an IFlightRepository interface with GetByIdAsync, AddAsync, UpdateAsync methods. Then implement it using EF Core DbContext.
```

### Need More Help?
- Check official docs: [EF Core Docs](https://learn.microsoft.com/en-us/ef/core/)
- Ask Copilot for examples: "Show me an example of [concept] in C#"
- Review the sample data: Look at `../../data/flights.csv` structure
