# 1 - Create model and data layer (Advanced)

Goal
Design a resilient domain model and data access layer for flight delay data. Aim for clear boundaries between domain, persistence, and API models. Consider DDD patterns where appropriate.

Tasks
1. Domain model
   - Design entities: Flight, Airline, Airport, FlightStatus, DelayEvent. Include value objects where helpful (e.g., Coordinates, FlightIdentifier).
   - Provide invariants: e.g., ScheduledDeparture must be before ActualDeparture when ActualDeparture is present. Model these checks in constructors or factory methods.

   Copilot prompt example:
   "Implement an immutable Flight value object with validation ensuring ScheduledDeparture is not null and DelayMinutes is non-negative. Use C# 11 record types and [JsonConstructor] for deserialization. Ensure you map Value Objects as Owned Types in your DbContext configuration so they flatten into the parent table."

2. Persistence
   - Add EF Core with a DbContext (FlightDelayDbContext). Configure fluent mappings to enforce constraints and indexes (e.g., index on Airline+FlightNumber+Date).
   - Implement repository interfaces (IFlightRepository) and at least one concrete implementation using EF Core.

   Copilot prompt example:
   "Generate an EF Core DbContext with DbSet<Flight> and a model configuration class that maps Flight to a table 'Flights', sets max length for strings, and creates an index on AirlineId and ScheduledDeparture."

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
