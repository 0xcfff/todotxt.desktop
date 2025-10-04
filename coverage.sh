#!/bin/bash

set -e

# Create reports/coverage directory if it doesn't exist
mkdir -p reports/coverage

dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=reports/coverage/baseline.xml
