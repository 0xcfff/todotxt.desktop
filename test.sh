#!/bin/bash

set -e

dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj
dotnet test src/TodoTxt.Avalonia.Tests/TodoTxt.Avalonia.Tests.csproj