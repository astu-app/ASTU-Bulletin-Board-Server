FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# устанавливаем curl для проверки состояния через healthcheck
RUN apt-get update && apt-get install -y curl
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src/BulletInBoardServer
COPY ["BulletInBoardServer/BulletInBoardServer/BulletInBoardServer.csproj", "BulletInBoardServer/"]
COPY ["BulletInBoardServer/BulletInBoardServer.Domain/BulletInBoardServer.Domain.csproj", "BulletInBoardServer.Domain/"]
COPY ["BulletInBoardServer/BulletInBoardServer.Services/BulletInBoardServer.Services.csproj", "BulletInBoardServer.Services/"]

WORKDIR /src/BulletInBoardServer.Controllers
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.Core/BulletInBoardServer.Controllers.Core.csproj", "BulletInBoardServer.Controllers.Core/"]
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.AnnouncementsController/src/BulletInBoardServer.Controllers.AnnouncementsController/BulletInBoardServer.Controllers.AnnouncementsController.csproj", "BulletInBoardServer.Controllers.AnnouncementsController/src/BulletInBoardServer.Controllers.AnnouncementsController/"]
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.SurveysController/src/BulletInBoardServer.Controllers.SurveysController/BulletInBoardServer.Controllers.SurveysController.csproj", "BulletInBoardServer.Controllers.SurveysController/src/BulletInBoardServer.Controllers.SurveysController/"]
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.UserGroupsController/src/BulletInBoardServer.Controllers.UserGroupsController/BulletInBoardServer.Controllers.UserGroupsController.csproj", "BulletInBoardServer.Controllers.UserGroupsController/src/BulletInBoardServer.Controllers.UserGroupsController/"]
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.UsersController/src/BulletInBoardServer.Controllers.UsersController/BulletInBoardServer.Controllers.UsersController.csproj", "BulletInBoardServer.Controllers.UsersController/src/BulletInBoardServer.Controllers.UsersController/"]

WORKDIR /src
RUN dotnet restore "BulletInBoardServer/BulletInBoardServer/BulletInBoardServer.csproj"
WORKDIR /src
COPY ["BulletInBoardServer/BulletInBoardServer/", "BulletInBoardServer/BulletInBoardServer/"]
COPY ["BulletInBoardServer/BulletInBoardServer.Domain/", "BulletInBoardServer/BulletInBoardServer.Domain/"]
COPY ["BulletInBoardServer/BulletInBoardServer.Services/", "BulletInBoardServer/BulletInBoardServer.Services/"]
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.Core/", "BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.Core/"]
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.AnnouncementsController/src/BulletInBoardServer.Controllers.AnnouncementsController", "BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.AnnouncementsController/src/BulletInBoardServer.Controllers.AnnouncementsController"]
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.SurveysController/src/BulletInBoardServer.Controllers.SurveysController", "BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.SurveysController/src/BulletInBoardServer.Controllers.SurveysController"]
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.UserGroupsController/src/BulletInBoardServer.Controllers.UserGroupsController", "BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.UserGroupsController/src/BulletInBoardServer.Controllers.UserGroupsController"]
COPY ["BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.UsersController/src/BulletInBoardServer.Controllers.UsersController", "BulletInBoardServer.Controllers/BulletInBoardServer.Controllers.UsersController/src/BulletInBoardServer.Controllers.UsersController"]
WORKDIR /src/BulletInBoardServer/BulletInBoardServer
RUN dotnet build "BulletInBoardServer.csproj" -c $BUILD_CONFIGURATION -o /app/build -maxcpucount:1

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BulletInBoardServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# RUN dotnet publish "BulletInBoardServer.Controllers.SurveysController.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# RUN dotnet publish "BulletInBoardServer.Controllers.SurveysController.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# RUN dotnet publish "BulletInBoardServer.Controllers.UserGroupsController.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
# копируем сертификат безопасности
#WORKDIR /
#COPY ["BulletInBoardServer/BulletInBoardServer/certificates/https/aspnetapp.pfx", "https/"] 
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletInBoardServer.dll"]
