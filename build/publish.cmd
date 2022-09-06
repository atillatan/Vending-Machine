@echo off
setlocal EnableDelayedExpansion

@echo "Reading version information"
set c=0
set version=""

for /f "tokens=1,2 delims=:, " %%a in (' find ":" ^< "../src/Machine.CLI/appsettings.production.json" ') do (
  if "%%~a"=="version" (
    set version=%%~b
  )
)

SET ASPNETCORE_ENVIRONMENT=Development
SET BUILD_CONFIGURATION=Release
SET cpycmd=/D /I /E /F /Y /H /R 
SET PUBLISH_PATH=Build
SET BUILD_VERSION={ "version":"%version%"}
SET PACKAGEVERSION=2.1.0
SET DOTNET_CLI_TELEMETRY_OPTOUT=1
cd ..
SET WORKSPACE=%CD%

if NOT "%1" == "" (SET PUBLISH_PATH=%1)
if NOT "%2" == "" (SET BUILD_VERSION=%2)

@echo ============ CONFIG ============
@echo ASPNETCORE_ENVIRONMENT:%ASPNETCORE_ENVIRONMENT%
@echo BUILD_CONFIGURATION:%BUILD_CONFIGURATION%
@echo PUBLISH_PATH:%PUBLISH_PATH% 
@echo BUILD_VERSION:%BUILD_VERSION%
@echo WORKSPACE:%WORKSPACE%
@echo ================================

cd %WORKSPACE%
dotnet clean .

@echo "Restore started..."
dotnet restore src/Machine.Domain
dotnet restore src/Machine.Application
dotnet restore src/Machine.Infrastructure
dotnet restore src/Machine.CLI
dotnet restore src/Machine.API

@echo "Build started..."
dotnet build src/Machine.Domain
dotnet build src/Machine.Application
dotnet build src/Machine.Infrastructure
dotnet build src/Machine.CLI
dotnet build src/Machine.API

@echo "Tests started..."
dotnet test tests/Machine.Infrastructure.UnitTests/Machine.Infrastructure.UnitTests.csproj

@echo "Build CLI nuget package started..."
dotnet pack src/Machine.CLI --include-source --output %PUBLISH_PATH%/machine-cli-nuget --verbosity n /p:PackageVersion=%PACKAGEVERSION% -c Pack

@echo "Publish CLI started..."
dotnet publish src/Machine.CLI -r win10-x64 --self-contained -c %BUILD_CONFIGURATION% --output "%PUBLISH_PATH%/windows-cli"
xcopy "config" "%PUBLISH_PATH%/windows-cli/config"  %cpycmd%

echo %BUILD_VERSION% > "%PUBLISH_PATH%\windows-api\version.json"

@echo "Build API nuget package started..."
dotnet pack src/Machine.API --include-source --output %PUBLISH_PATH%/nuget --verbosity n /p:PackageVersion=%PACKAGEVERSION% -c Pack

@echo "Publish API started..."
dotnet publish src/Machine.API -r win10-x64 --self-contained -c %BUILD_CONFIGURATION% --output "%PUBLISH_PATH%/windows-api"
xcopy "config" "%PUBLISH_PATH%/windows-api/config"  %cpycmd%

echo %BUILD_VERSION% > "%PUBLISH_PATH%\windows-api\version.json"


