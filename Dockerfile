# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY . /build
RUN dotnet restore ChatWatchApp.csproj
RUN dotnet publish ChatWatchApp.csproj -c release -o /app --no-restore --self-contained

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN mkdir /app
RUN adduser --disabled-password --home /home/container container
USER container
ENV USER=container HOME=/home/container

WORKDIR /home/container
COPY --from=build /app /app
COPY ./pterodactyl.sh /entrypoint.sh

ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["/usr/bin/bash", "/entrypoint.sh"]

LABEL org.opencontainers.image.source="https://github.com/raineycat/ChatWatch" 
LABEL org.opencontainers.image.description="Minecraft chat monitoring server"
