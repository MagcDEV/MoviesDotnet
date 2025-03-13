# README

## Overview
This project provides a simple Movies API built with ASP.NET Core. It uses a PostgreSQL database, configured via Docker Compose.

## Getting Started
1. Clone the repository.
2. Run the database container:
   • docker-compose up -d
3. Restore packages and build:
   • dotnet restore
   • dotnet build
4. Start the application:
   • dotnet run --project Movies.Api

## Usage
Access the API at http://localhost:5000 (or as configured). Swagger is available at /swagger.
