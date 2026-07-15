# Contributing to GeForceNowWindowMover

## Before you start
- Check existing issues to avoid duplicate work.
- Keep changes focused and scoped to one purpose.
- Open an issue first for significant feature changes.

## Development setup
### Prerequisites
- Windows
- .NET 9 SDK (matching `net9.0-windows10.0.26100.0`)
- Visual Studio 2022 or `dotnet` CLI
- Docker Desktop with Windows containers enabled (optional, for container workflow)

### Local setup
1. Fork and clone the repository.
2. Restore dependencies:
   - `dotnet restore .\GFNWindowMover.csproj`
3. Build:
   - `dotnet build .\GeForceNowWindowMover.sln -c Release`

### Optional Docker build (Windows container)
- `docker build -t gfn-window-mover:dev .`

The project targets Windows desktop APIs, so Docker usage requires a Windows container host.

## Code style
- Follow repository rules in `.editorconfig`.
- Keep naming and structure consistent with `Program.cs`, `UI.cs`, and `Utilities\`.
- Run your formatter/lint tooling in the IDE before opening a PR.

## Running tests
- Run all tests in the solution:
  - `dotnet test .\GeForceNowWindowMover.sln -c Release`
- Run only the main test project (same scope as CI):
  - `dotnet test .\GFNWindowMover.Tests\GFNWindowMover.Tests.csproj -c Release --no-build --verbosity normal`

## Pull requests
- Use a descriptive title and explain what changed and why.
- Reference related issue numbers when applicable.
- Ensure CI is green.
- Keep commits and code style consistent with the existing project.

## Reporting bugs
Please use the bug issue template and include:
- Reproduction steps
- Expected vs actual behavior
- Windows version and app version
- Screenshots or logs if helpful
