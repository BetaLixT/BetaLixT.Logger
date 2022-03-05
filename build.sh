#!/bin/bash

set -e

dotnet restore

dotnet build -c Release src/BetaLixT.Logger/BetaLixT.Logger.csproj 
