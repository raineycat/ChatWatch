#!/usr/bin/sh

# Entrypoint script for running under a pterodactyl container
# https://pterodactyl.io/community/config/eggs/creating_a_custom_image.html

cd /home/container/cw

echo "!!! STARTING !!!"
echo "ASP.NET: $(dotnet --list-runtimes | grep AspNet | awk -F ' ' '{print $2}')"

echo "!!! RUNNING COMMAND !!!"

# Replace Startup Variables
MODIFIED_STARTUP="${STARTUP} --ConnectionStrings:DefaultConnection ${DbConnection} --CWServerConfig:ServerName ${ServerName} --CWServerConfig:IngestToken ${IngestToken}"
echo ":/home/container$ ${MODIFIED_STARTUP}"

# Run the Server
${MODIFIED_STARTUP}