# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore
COPY Backend/TelemetryApi/TelemetryApi.csproj Backend/TelemetryApi/
COPY Backend/Models Backend/Models
COPY Backend/Services Backend/Services
RUN dotnet restore Backend/TelemetryApi/TelemetryApi.csproj

# Copy the full source and publish
COPY Backend Backend
WORKDIR /src/Backend/TelemetryApi
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5010
EXPOSE 5010

ENTRYPOINT ["dotnet", "TelemetryApi.dll"]
