# 2 - Create the API (Advanced)

Goal
Build a robust, versioned ASP.NET Core Web API exposing endpoints for querying flights, reporting delays, and operational endpoints for admins. Focus on correctness, performance, and maintainability.

Core features to implement
1. Read model endpoints
   - GET /v1/flights?airline=XXX&from=YYY&date=YYYY-MM-DD
   - Support pagination, filtering by delay thresholds, sorting, and projection (select a subset of fields)

2. Command endpoints
   - POST /v1/flights/{id}/report-delay — accepts a command to register a new DelayEvent; ensure idempotency
   - Use proper request validation with FluentValidation or similar

3. Operational and health endpoints
   - /health/live and /health/ready
   - /metrics (Prometheus) if time permits

Architecture and patterns
- Use dependency injection and layered architecture. Separate controllers from application services and repositories.
- Add middleware for structured logging, exception handling (problem-details), and a correlation id.
- Implement rate limiting (per-IP or per-API key) using a middleware or built-in ASP.NET features.

Security
- Protect admin/command endpoints with authentication (JWT or API key). For workshop purposes, a simple API key middleware is acceptable.

Copilot prompt examples
- "Create an ASP.NET Core controller named FlightsController with endpoints: GET /v1/flights and POST /v1/flights/{id}/report-delay. Use MediatR for command handling and return ProblemDetails on errors."
- "Write middleware that adds a CorrelationId to each request, logs it, and returns the same id in a response header 'X-Correlation-Id'."
- "Create a validator for ReportDelayCommand using FluentValidation, ensuring DelayMinutes is positive and FlightId exists."

Advanced features (pick at least three)
- Implement background processing: a hosted service that consumes a queue (e.g., Channel<T>) to process DelayEvents and update aggregates.
- Add caching for read-heavy endpoints using IMemoryCache or Redis (optional: demonstrate stale-while-revalidate).
- Add a CQRS + mediator setup: queries go to read models, commands go through handlers and publish events.
- Implement API versioning using Microsoft.AspNetCore.Mvc.Versioning for backward compatibility.
- Add resilience patterns: Use Polly for circuit breakers and retries on external calls (e.g., if integrating with an external flight API). In .NET 8, consider Microsoft.Extensions.Http.Resilience for standard resilient HTTP requests.
- Implement distributed tracing with OpenTelemetry and export to Jaeger or Zipkin.

Testing
- Unit tests: controller actions, validation, and command handlers.
- Integration tests: use WebApplicationFactory<TEntryPoint> to test real controllers and middleware.

Performance and telemetry
- Add basic OpenTelemetry tracing and metrics.
- Run a quick load test (wrk or k6) against the read endpoint and identify bottlenecks.

Checkpoint
- Core API endpoints implemented and covered by unit tests.
- Security for command endpoints implemented and manually tested.

Deliverable
- A versioned API with controllers, DTOs, validation, middleware, and at least one background worker or caching layer.
