# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /Vending-Machine

# Environment
ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_GENERATE_ASPNET_CERTIFICATE=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
# ENV ASPNETCORE_URLS=http://+:8100
# EXPOSE 8100
# EXPOSE 8101

# RUN apt-get update
# RUN apt-get -y install curl libcurl4-openssl-dev

# Copy everything into Docker
COPY . .

# Publish 
WORKDIR /Vending-Machine/build
RUN chmod +x publish-linux.sh
RUN ./publish-linux.sh

# Run
WORKDIR /Vending-Machine/build/linux-cli
# RUN dotnet dev-certs https
# ENTRYPOINT ["./vmachine"]
ENTRYPOINT ["sh"]

# Docker Commands
# 1. create image
#   $ docker build --tag vmachine --file Dockerfile . 
# 2. create/run container
#   $ docker create --name vmachine1 vmachine
#   $ docker run --rm -d --name vmachine1 -h vmachine1 -t vmachine:latest