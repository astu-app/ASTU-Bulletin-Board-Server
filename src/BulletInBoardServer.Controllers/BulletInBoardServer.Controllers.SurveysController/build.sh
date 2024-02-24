#!/usr/bin/env bash
#
# Generated by: https://openapi-generator.tech
#

dotnet restore src/BulletInBoardServer.Controllers.SurveysController/ && \
    dotnet build src/BulletInBoardServer.Controllers.SurveysController/ && \
    echo "Now, run the following to start the project: dotnet run -p src/BulletInBoardServer.Controllers.SurveysController/BulletInBoardServer.Controllers.SurveysController.csproj --launch-profile web"
