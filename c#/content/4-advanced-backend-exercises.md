# 4 - Advanced Backend Exercises (Optional Extensions)

These are additional, highly advanced exercises to extend the workshop for participants seeking deeper challenges. Each can be tackled independently and adds 1-2 hours to the timeline.

## Exercise A: Microservices Decomposition
- Split the monolith into microservices: e.g., FlightService (queries), DelayReportingService (commands), and NotificationService (events).
- Use message queues (RabbitMQ or Azure Service Bus) for inter-service communication.
- Implement service discovery and API gateways with Ocelot or YARP.

Copilot prompt: "Design a microservices architecture diagram and implement a simple message publisher/subscriber using MassTransit for DelayEvent notifications."

## Exercise B: Event Sourcing with Marten
- Replace EF Core with Marten for event sourcing: Store DelayEvents as events and project read models.
- Implement event versioning and upcasters for schema evolution.
- Add projections for real-time dashboards.

Warning: Requires a running PostgreSQL container (use Docker).

Copilot prompt: "Implement an event-sourced aggregate for Flight using Marten, with commands to report delays and queries to get current state."

## Exercise C: Advanced Security and Compliance
- Implement OAuth 2.0 / OpenID Connect with IdentityServer or Duende.
- Add data encryption at rest and in transit.
- Implement audit logging for all changes, complying with GDPR-like requirements.

Copilot prompt: "Add JWT authentication middleware to ASP.NET Core and a policy for role-based access to admin endpoints."

## Exercise D: Performance Optimization and Scaling
- Implement database sharding or partitioning for large datasets.
- Add response compression and HTTP/2 support.
- Profile with dotnet-trace and optimize hot paths.

Copilot prompt: "Write a benchmark using BenchmarkDotNet to compare EF Core query performance with and without compiled queries."

## Exercise E: Observability and Monitoring
- Set up full OpenTelemetry stack: traces, metrics, logs exported to Prometheus/Grafana.
- Implement health checks with custom probes.
- Add alerting rules for high latency or error rates.

Copilot prompt: "Configure OpenTelemetry in ASP.NET Core to export traces to Jaeger and metrics to Prometheus."

Deliverable
- Documented implementations of chosen exercises, with tests and Copilot usage notes.