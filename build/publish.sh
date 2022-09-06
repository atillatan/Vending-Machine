!/bin/sh
Build.sh

ver=$(cat ../src/Machine.CLI/appsettings.production.json|awk -F'"' '/"version": ".+"/{ print $4; exit; }')
export ASPNETCORE_ENVIRONMENT=Development
export BUILD_CONFIGURATION=Release
export PUBLISH_PATH=build
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
echo PUBLISH_PATH:build
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
rm -rf tests/Machine.Infrastructure.UnitTests/bin tests/Machine.Infrastructure.UnitTests/obj


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
dotnet pack src/Machine.CLI --include-source --output build/machine-cli-nuget --verbosity n /p:PackageVersion=$PACKAGEVERSION -c Pack

@echo "Publish CLI started..."
dotnet publish src/Machine.CLI -r osx-x64 --self-contained -c $BUILD_CONFIGURATION --output "build/macos-cli"

echo %BUILD_VERSION% > "build/macos-api/version.json"

@echo "Build API nuget package started..."
dotnet pack src/Machine.API --include-source --output build/nuget --verbosity n /p:PackageVersion=$PACKAGEVERSION -c Pack

@echo "Publish API started..."
dotnet publish src/Machine.API -r osx-x64 --self-contained -c $BUILD_CONFIGURATION --output "build/macos-api"

cp -R config build/macos-cli
cp -R config build/macos-api


echo $BUILD_VERSION > version.json
mv version.json build/macos-cli
echo $BUILD_VERSION > version.json
mv version.json build/macos-api
