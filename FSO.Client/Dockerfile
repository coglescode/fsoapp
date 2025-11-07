# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FSO.Client/FSO.Client.csproj", "FSO.Client/"]
RUN dotnet restore "./FSO.Client/FSO.Client.csproj"
COPY . .
WORKDIR "/src/FSO.Client"
RUN dotnet build "./FSO.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build
###
#
# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FSO.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false



# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Set environment variable to enable globalization support
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Install ICU package for globalization support
RUN apk add --no-cache icu-data-full icu-libs

WORKDIR /app

USER $APP_UID

EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FSO.Client.dll"]