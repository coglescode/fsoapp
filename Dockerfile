# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FSO.App/FSO.App.csproj", "FSO.App/"]
RUN dotnet restore "FSO.App/FSO.App.csproj"
RUN dotnet tool install --global dotnet-ef

COPY . .
WORKDIR "/src/FSO.App"
RUN dotnet build "./FSO.App.csproj" -c $BUILD_CONFIGURATION -o /app/build


# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FSO.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# Final runtime image
FROM base AS final

# Set environment variable to enable globalization support
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8


# Install ICU package for globalization support
RUN apk add --no-cache icu-libs

WORKDIR /app

#
#USER $APP_UID
#
#EXPOSE 8080
#EXPOSE 8081

#FROM base AS final

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FSO.App.dll"]
