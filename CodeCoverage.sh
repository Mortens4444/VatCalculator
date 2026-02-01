#!/bin/bash

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# Open report
if [[ "$OSTYPE" == "darwin"* ]]; then
    open coveragereport/index.html
else
    xdg-open coveragereport/index.html
fi