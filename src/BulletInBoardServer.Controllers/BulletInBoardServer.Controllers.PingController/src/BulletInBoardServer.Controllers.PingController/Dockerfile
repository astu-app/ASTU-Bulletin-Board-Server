#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Container we use for final publish
FROM mcr.microsoft.com/dotnet/core/aspnet:6.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build container
FROM mcr.microsoft.com/dotnet/core/sdk:6.0-buster AS build

# Copy the code into the container
WORKDIR /src
COPY ["src/BulletInBoardServer.Controllers.PingController/BulletInBoardServer.Controllers.PingController.csproj", "BulletInBoardServer.Controllers.PingController/"]

# NuGet restore
RUN dotnet restore "BulletInBoardServer.Controllers.PingController/BulletInBoardServer.Controllers.PingController.csproj"
COPY ["src/BulletInBoardServer.Controllers.PingController/", "BulletInBoardServer.Controllers.PingController/"]

# Build the API
WORKDIR "BulletInBoardServer.Controllers.PingController"
RUN dotnet build "BulletInBoardServer.Controllers.PingController.csproj" -c Release -o /app/build

# Publish it
FROM build AS publish
RUN dotnet publish "BulletInBoardServer.Controllers.PingController.csproj" -c Release -o /app/publish

# Make the final image for publishing
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletInBoardServer.Controllers.PingController.dll"]
