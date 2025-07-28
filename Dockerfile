# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project and restore
COPY BenchApp.csproj .
RUN dotnet restore

# Copy source and build
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Run the benchmark app
ENTRYPOINT ["dotnet", "BenchApp.dll"]
