# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY src/UserApi.Api/UserApi.Api.csproj ./src/UserApi.Api/
WORKDIR /src/src/UserApi.Api
RUN dotnet restore

# Copy everything else and build
COPY src/UserApi.Api/. ./src/UserApi.Api/
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create non-root user
RUN groupadd -r dotnet && useradd -r -g dotnet dotnet

# Copy published app
COPY --from=publish /app/publish .

# Change ownership and switch to non-root user
RUN chown -R dotnet:dotnet /app
USER dotnet

EXPOSE 8080

# Set environment variable for .NET 9
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "UserApi.Api.dll"]