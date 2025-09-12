# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["DotNetTutor.Api/DotNetTutor.Api.csproj", "DotNetTutor.Api/"]
RUN dotnet restore "DotNetTutor.Api/DotNetTutor.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/DotNetTutor.Api"
RUN dotnet build "DotNetTutor.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "DotNetTutor.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install SQLite (since the app uses SQLite)
RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

# Copy the published application
COPY --from=publish /app/publish .

# Create directory for SQLite database
RUN mkdir -p /app/data

# Expose port 80
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "DotNetTutor.Api.dll"]

