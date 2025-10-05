FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
# Path relative to the root directory of the container's file system.
WORKDIR /app 
EXPOSE 8080
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src 
COPY Capstone.sln ./ 
COPY FSO.Client/FSO.Client.csproj FSO.Client/
COPY FSO.API/FSO.API.csproj FSO.API/
#RUN dotnet nuget locals all --clear
RUN dotnet restore FSO.Client/FSO.Client.csproj


# Build and publish a release
# w WORKDIR /src/FSO.Client
WORKDIR /src
COPY . .

RUN dotnet build FSO.Client/FSO.Client.csproj -c Release -o /app/build
RUN dotnet build FSO.API/FSO.API.csproj -c Release -o /app/build


FROM build AS publish
RUN dotnet publish FSO.Client/FSO.Client.csproj -c Release -o /app/publish
RUN dotnet publish FSO.API/FSO.API.csproj -c Release -o /app/publish


# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "FSO.Client.dll"]