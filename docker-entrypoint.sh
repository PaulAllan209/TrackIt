#!/bin/bash
set -e

# Run the application with the secrets JSON file
exec dotnet TrackIt.Server.dll --secretsfile /app/secrets.json