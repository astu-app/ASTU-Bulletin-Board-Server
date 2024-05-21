FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BulletInBoardServer/BulletInBoardServer.csproj", "BulletInBoardServer/"]
RUN dotnet restore "BulletInBoardServer/BulletInBoardServer.csproj"
COPY . .
WORKDIR "/src/BulletInBoardServer"
RUN dotnet build "BulletInBoardServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BulletInBoardServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletInBoardServer.dll"]
