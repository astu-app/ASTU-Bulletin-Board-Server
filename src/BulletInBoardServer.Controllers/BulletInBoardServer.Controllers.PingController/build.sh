#!/usr/bin/env bash
#
# Generated by: https://openapi-generator.tech
#

dotnet restore src/BulletInBoardServer.Controllers.PingController/ && \
    dotnet build src/BulletInBoardServer.Controllers.PingController/ && \
    echo "Now, run the following to start the project: dotnet run -p src/BulletInBoardServer.Controllers.PingController/BulletInBoardServer.Controllers.PingController.csproj --launch-profile web"