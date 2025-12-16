# 3 - Integration & Verification

This step is optional and focuses on backend validation rather than frontend development. The emphasis remains on backend challenges.

Goals
- Build tools to test and integrate with the backend API. Keep it simple: console apps, scripts, or API clients for validation.

Suggestions
- Console client (C#): Use System.CommandLine to create a CLI that lists flights and reports delays. This is great for testing idempotency and command handling.
- API testing scripts: Use HttpClient in a console app or PowerShell scripts to automate API calls and assertions.
- Postman/Newman collections: Document API endpoints in a Postman collection for automated testing.

Copilot prompt examples
- "Create a .NET console app using System.CommandLine that supports commands: list-flights --airline XX --date YYYY-MM-DD and report-delay --id ID --minutes N. Use HttpClient and read base URL from environment variable."
- "Write a script using HttpClient to perform a load test: send 100 concurrent GET requests to /v1/flights and measure response times."

Testing
- Use the client to exercise workflows: import data -> call GET -> post delay -> assert updated values via GET.
- Integrate with backend: Test rate limiting, authentication, and error handling.

Deliverable
- A small client project under `c#/clients/` or scripts for API testing and validation.
