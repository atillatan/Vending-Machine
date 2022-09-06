#!/bin/sh
export ASPNETCORE_ENVIRONMENT="Development"
dotnet restore ../
dotnet build -c Debug ../
dotnet test ../tests/Machine.Infrastructure.UnitTests/Machine.Infrastructure.UnitTests.csproj
