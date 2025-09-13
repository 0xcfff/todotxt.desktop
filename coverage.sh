#!/bin/bash

set -e

dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=teamcity
