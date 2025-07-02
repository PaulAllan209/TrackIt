FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Library needed for SQL Server authentication
# Make sure to install libraries like this in the final runtime image (base/final stage)
RUN apt-get update && apt-get install -y libgssapi-krb5-2

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src



# Copy proj files
COPY ["TrackIt.Server/TrackIt.Server.csproj", "TrackIt.Server/"]
COPY ["TrackIt.Core/TrackIt.Core.csproj", "TrackIt.Core/"]
# Restore as distinct layers
RUN dotnet restore "TrackIt.Core/TrackIt.Core.csproj"

#Copy all source code
COPY ["TrackIt.Server", "TrackIt.Server"]
COPY ["TrackIt.Core", "TrackIt.Core"]

WORKDIR "/src/TrackIt.Server"
# Build and publish a release
RUN dotnet publish -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Copy the entrypoint script
COPY docker-entrypoint.sh /app/
RUN chmod +x /app/docker-entrypoint.sh

ENTRYPOINT ["/app/docker-entrypoint.sh"]