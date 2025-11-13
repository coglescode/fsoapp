# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Build stage
# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FSO.App.csproj", "."]
RUN dotnet restore "FSO.App.csproj"
# RUN dotnet tool install --global dotnet-ef --version 9.0.*
COPY . .
WORKDIR "/src/"
RUN dotnet build "FSO.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FSO.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

#
# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FSO.App.dll"]


# This the default image por FSO.App solution.