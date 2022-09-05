# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /Vending-Machine

# Environment
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_GENERATE_ASPNET_CERTIFICATE=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:8100
EXPOSE 8100
EXPOSE 8101

# Copy everything into Docker
COPY . .

# Publish 
WORKDIR /Vending-Machine/Config
RUN chmod +x Publish.sh
RUN ./Publish.sh

# Run
WORKDIR /Vending-Machine/build/linux
# RUN dotnet dev-certs https
ENTRYPOINT ["./Machine.API"]
#ENTRYPOINT ["sh"]

 