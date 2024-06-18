#!/usr/bin/sh

# Entrypoint script for running under a pterodactyl container
# https://pterodactyl.io/community/config/eggs/creating_a_custom_image.html

cd /app

echo "!!! STARTING !!!"
echo "ASP.NET: $(dotnet --list-runtimes | grep AspNet | awk -F ' ' '{print $2}')"
echo "Original command: ${STARTUP}"
echo ""

echo "!!! RUNNING COMMAND !!!"

# Replace Startup Variables
VARIABLED_STARTUP=`eval echo $(echo ${STARTUP} | sed -e 's/{{/${/g' -e 's/}}/}/g')`
echo "COMMAND: ${pwd}$ ${VARIABLED_STARTUP}"

# Run the Server
${VARIABLED_STARTUP}

echo "\n!!! DONE !!!"

