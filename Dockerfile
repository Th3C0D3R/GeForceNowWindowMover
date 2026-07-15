FROM mcr.microsoft.com/dotnet/sdk:9.0-windowsservercore-ltsc2022 AS build
WORKDIR C:\src

COPY GFNWindowMover.csproj .
RUN dotnet restore .\GFNWindowMover.csproj

COPY . .
RUN dotnet publish .\GFNWindowMover.csproj -c Release -o C:\app\publish --no-restore

FROM mcr.microsoft.com/dotnet/windowsdesktop:9.0-windowsservercore-ltsc2022 AS runtime
WORKDIR C:\app
COPY --from=build C:\app\publish .

ENTRYPOINT ["GFNWindowMover.exe"]
