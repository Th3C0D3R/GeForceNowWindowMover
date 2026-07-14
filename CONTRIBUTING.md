# Contributing to GeForceNowWindowMover

## Before you start
- Check existing issues to avoid duplicate work.
- Keep changes focused and scoped to one purpose.
- Open an issue first for significant feature changes.

## Development setup
1. Fork and clone the repository.
2. Build locally:
   - `dotnet restore`
   - `dotnet build .\GeForceNowWindowMover.sln -c Release`
3. Run tests:
   - `dotnet test .\GFNWindowMover.Tests\GFNWindowMover.Tests.csproj -c Release`

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
