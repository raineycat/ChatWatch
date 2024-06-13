# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY . /build
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ChatWatchApp.dll"]

LABEL org.opencontainers.image.source = "https://github.com/sbcomputertech/ChatWatch" 
LABEL org.opencontainers.image.description = "Minecraft chat monitoring server"