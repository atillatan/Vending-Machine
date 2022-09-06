#!/bin/sh
#Build.sh

ver=$(cat ../src/Machine.CLI/appsettings.production.json|awk -F'"' '/"version": ".+"/{ print $4; exit; }')
export ASPNETCORE_ENVIRONMENT=Development
export BUILD_CONFIGURATION=Release
export PUBLISH_PATH=Build
export BUILD_VERSION='{ "version":"'${ver}'" }'
export PACKAGEVERSION=2.1.0
export DOTNET_CLI_TELEMETRY_OPTOUT=1
cd ..
export WORKSPACE=$PWD

if [ ! -z "$1" ];
then 
    export PUBLISH_PATH=$1
fi

if [ ! -z "$2" ];
then 
    export BUILD_VERSION=$2
fi

echo ============ CONFIG ============
echo ASPNETCORE_ENVIRONMENT:$ASPNETCORE_ENVIRONMENT
echo BUILD_CONFIGURATION:$BUILD_CONFIGURATION
echo PUBLISH_PATH:$PUBLISH_PATH
echo BUILD_VERSION:$BUILD_VERSION
echo WORKSPACE:$WORKSPACE
echo ================================

ls -la
dotnet clean .

# remove pre-compiled files
rm -rf src/Machine.Domain/bin src/Machine.Domain/obj
rm -rf src/Machine.Application/bin src/Machine.Application/obj
rm -rf src/Machine.Infrastructure/bin src/Machine.Infrastructure/obj
rm -rf src/Machine.API/bin src/Machine.API/obj
rm -rf tests/Machine.API.UnitTests/bin tests/Machine.API.UnitTests/obj
rm -rf Build/linux/*


echo "Restore started..."
#dotnet restore .
dotnet restore src/Machine.Domain
dotnet restore src/Machine.Application
dotnet restore src/Machine.Infrastructure
dotnet restore src/Machine.CLI
dotnet restore src/Machine.API

echo "Build started..."
#dotnet build 
dotnet build src/Machine.Domain
dotnet build src/Machine.Application
dotnet build src/Machine.Infrastructure
dotnet build src/Machine.CLI
dotnet build src/Machine.API

echo "Tests started..."
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

cp -RTp Config $PUBLISH_PATH/linux/Config
cp -RTp docs $PUBLISH_PATH/linux/wwwroot/docs
echo $BUILD_VERSION > version.json
mv version.json $PUBLISH_PATH/linux
