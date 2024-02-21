# BulletInBoardServer.Controllers.PingController - ASP.NET Core 6.0 Server

Проверка доступности сервиса

## Upgrade NuGet Packages

NuGet packages get frequently updated.

To upgrade this solution to the latest version of all NuGet packages, use the dotnet-outdated tool.


Install dotnet-outdated tool:

```
dotnet tool install --global dotnet-outdated-tool
```

Upgrade only to new minor versions of packages

```
dotnet outdated --upgrade --version-lock Major
```

Upgrade to all new versions of packages (more likely to include breaking API changes)

```
dotnet outdated --upgrade
```


## Run

Linux/OS X:

```
sh build.sh
```

Windows:

```
build.bat
```
## Run in Docker

```
cd src/BulletInBoardServer.Controllers.PingController
docker build -t bulletinboardserver.controllers.pingcontroller .
docker run -p 5000:8080 bulletinboardserver.controllers.pingcontroller
```
