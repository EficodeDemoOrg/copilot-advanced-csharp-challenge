# Advanced C# Backend Workshop (Copilot-focused)

This workshop is an advanced, multi-hour exercise for experienced C# developers. The objective is to build a production-quality backend API using modern .NET techniques while leveraging GitHub Copilot as a development assistant. The materials in this folder are intentionally challenging and open-ended to encourage exploration, design trade-offs, and advanced problem solving.

Estimated time: 4-8 hours (depending on experience and depth of optional extensions)

What's included
- Step-by-step instruction files in the c#/content folder: `0-get-started.md`, `1-create-model-data.md`, `2-create-api.md`, `3-create-integration-verification.md` (integration is optional/minimal), `4-advanced-backend-exercises.md` (for extra challenges)
- Hints and Copilot-friendly prompts embedded in each step
- Checkpoints and evaluation criteria for mentors

Goals
- Design a robust domain model and persistence layer using EF Core or other data-access patterns, including DDD aggregates and domain events
- Create a secure, well-tested ASP.NET Core Web API with async patterns, DI, middleware, and resilience patterns (circuit breakers, retries)
- Implement advanced features: background processing, idempotency, CQRS, event sourcing, API versioning, distributed caching, and telemetry
- Explore microservices concepts: service boundaries, message queues, and observability
- Use GitHub Copilot intentionally: craft prompts, iterate on generated code, and critically review its suggestions

How to use this folder
1. Read `0-get-started.md` to set up the environment and use the recommended starter scaffold for efficient sessions.
2. Work through `1-create-model-data.md` and `2-create-api.md` for the core backend work.
3. Optionally complete `3-create-integration-verification.md` if you want to test and integrate the API.
4. Tackle `4-advanced-backend-exercises.md` for deeper challenges.

Copilot guidance
- Use the provided example prompts in each step. Start with small prompts (function-level) and progress to higher-level design prompts (architecture, tests).
- Treat Copilot as a pair programmer: accept small suggestions, but run tests and statically review generated code. Add unit tests and integration tests that validate behavior.
- Context Awareness: Copilot works best when related files are open. Keep your Flight entity open in a tab when writing your FlightRepository to help Copilot infer properties.

Assessment / Checkpoints
- Checkpoint 1 (1-2 hours): Domain model and migrations exist; basic CRUD endpoints implemented and tested.
- Checkpoint 2 (3-5 hours): Add advanced features (background worker, caching, idempotency, CQRS, event sourcing) and secure the API (JWT or API keys).
- Final (4-8 hours): End-to-end tests, documentation, perf checks, telemetry, and retrospective: record what Copilot helped with and what required manual changes. Include a "Vulnerability Check": Ask Copilot to review its own code for security flaws (e.g., Prompt: "Review this controller method for security vulnerabilities and suggest fixes.").
