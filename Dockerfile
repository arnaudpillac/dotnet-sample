# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY dotnet-thread-test.csproj ./
RUN dotnet restore

# Copy the rest of the app
COPY . ./
RUN dotnet build -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "dotnet-thread-test.dll"]
