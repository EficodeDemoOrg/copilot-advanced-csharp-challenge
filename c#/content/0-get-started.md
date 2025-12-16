# 0 - Get started (Advanced C# Backend Workshop)

## Overview
This workshop targets experienced C# developers. The emphasis is on backend engineering: robust domain modelling, data access, scalable API design, testing, and operational concerns. You'll be encouraged to use GitHub Copilot as your pair programmer — craft prompts, accept/adjust suggestions, and critically review outputs.

## Create and start the project

To start creating code you'll of course need to get the code. This repository is setup as a template. Let's create an instance of the repository in the appropriate organization or individual account, and start the codespace.

1. Navigate to the [root of the repository on GitHub](../).
2. Select **Use this template** > **Create a new repository**.
3. Enter the appropriate information to configure the name and location of the repository. If a specific organization has been defined for your event, use that as the **owner**. (If you're unsure, ask a mentor).
4. Select **Create repository** to create the repository.
5. Once the repository is created, open it in GitHub Codespaces by selecting **Code** > **Codespaces** > **Create a codespace on main** (indicated by the **+**).
6. The codespace may take a few minutes to setup the first time. It contains everything needed for the workshop, including .NET 8. It doesn't yet contain the extension for GitHub Copilot, which you'll install next.

## Install the extension

GitHub Copilot has extensions for Visual Studio, Visual Studio Code, NeoVIM and the JetBrains suite of IDEs. Because the browser-based version of Visual Studio Code for Codespaces is, well, Visual Studio Code, you can install the extension!

1. Open the command pallette by pressing <kbd>F1</kbd>.
2. In the command pallette window, type **Install extensions**.
3. Select **Extensions: Install extensions**.
4. Type **GitHub Copilot** in the newly opened extensions window.
5. Select **Install** on **GitHub Copilot** to install the extension.
6. If prompted, reconnect to the codespace.

Duration: 20-45 minutes to set up; full workshop is 3-6 hours.

## Prerequisites
- .NET 8 SDK installed (LTS version; .NET 7 is end-of-support)
- Advanced knowledge of ASP.NET Core, EF Core, async/await, DI, unit testing, and patterns like CQRS, event sourcing, and resilience (Polly)
- Recommended: Docker installed for running databases locally (needed for extensions like Marten or Redis); familiarity with OpenTelemetry or Prometheus for telemetry

Workshop modes
- Starter scaffold: Recommended for 4-hour sessions to avoid setup delays. The instructor will provide a minimal starter project scaffold in `c#/starter/`. Use this to focus on advanced tasks.
- From-scratch: For longer sessions or advanced users, create a new ASP.NET Core Web API project and implement the tasks below.

## Initial setup (from-scratch)
1. Create a directory for the exercise: mkdir -p csharp-workshop && cd csharp-workshop
2. Create a solution and web API project:

   dotnet new sln -n FlightDelayWorkshop
   dotnet new webapi -n FlightDelay.Api
   dotnet sln add FlightDelay.Api/FlightDelay.Api.csproj

3. Add a test project:

   dotnet new xunit -n FlightDelay.Tests
   dotnet add FlightDelay.Tests/FlightDelay.Tests.csproj reference FlightDelay.Api/FlightDelay.Api.csproj
   dotnet sln add FlightDelay.Tests/FlightDelay.Tests.csproj

4. Build and test the setup locally.

How to use Copilot in this workshop
- Start with granular prompts: "Write an EF Core entity for Flight with properties: Id, Airline, Origin, Destination, ScheduledDeparture, ActualDeparture, DelayMinutes".
- Ask Copilot to generate unit tests for a single method before accepting implementation.
- When adding middleware, prompt for examples (logging, correlation id, error handling) and inspect generated code carefully.

Checkpoint (after setup)
- Solution builds: dotnet build
- Test project created and runnable: dotnet test

## Next steps

You've now got the development environment created and started! You're all set and ready to start writing code. So, let's [begin working with the model](./1-create-model-data.md).

Deliverable for this step
- A working solution that builds and a test project that runs locally.

---

## Getting Unstuck

### Problem: "Codespace won't start or is slow"
**Solutions**:
- Wait 2-3 minutes on first launch (installing .NET 8 SDK)
- Check your GitHub Codespaces quota (free tier has limits)
- Alternative: Clone repo locally and use VS Code with Docker

### Problem: "GitHub Copilot extension not activating"
**Common fixes**:
1. Ensure you have an active GitHub Copilot subscription
2. Sign in to GitHub in VS Code (bottom left account icon)
3. Reload window: F1 → "Developer: Reload Window"
4. Check extension is enabled: Extensions panel → GitHub Copilot → Enable

### Problem: ".NET SDK not found"
**For Codespaces**: Should be pre-installed. Verify with `dotnet --version`

**For local setup**:
```bash
# Install .NET 8 SDK from https://dotnet.microsoft.com/download
dotnet --version  # Should show 8.0.x
```

### Problem: "dotnet build fails on starter project"
**Check**:
1. You're in the correct directory: `cd c#/starter`
2. Solution file exists: `ls *.sln`
3. Restore packages: `dotnet restore`

**Commands**:
```bash
cd /workspaces/copilot-advanced-csharp-challenge/c#/starter
dotnet restore
dotnet build
```

### Problem: "I'm new to GitHub Copilot - how do I use it?"
**Quick tips**:
1. **Inline suggestions**: Start typing, Copilot suggests completions (Tab to accept, Esc to dismiss)
2. **Comment-driven**: Write a comment describing what you want, Copilot generates code
   ```csharp
   // Create a method to validate email format using regex
   ```
3. **Chat**: Open Copilot Chat (Ctrl+Shift+I or Cmd+Shift+I) and ask questions
4. **Context**: Open related files in tabs - Copilot uses them as context

**Practice prompt**:
```
Create a simple Hello World endpoint in Program.cs that returns a JSON response with a message property.
```

### Using Starter vs From-Scratch
**Use starter if**:
- Time-limited workshop (4 hours)
- You want to focus on domain modeling and EF Core
- First time with Copilot-assisted development

**Start from scratch if**:
- 6+ hour workshop
- You want full control and practice project setup
- Experienced with .NET and want the challenge
