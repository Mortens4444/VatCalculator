Remove-Item -Recurse -Force VatCalculator.Api.Tests\TestResults, coveragereport -ErrorAction SilentlyContinue
dotnet test --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Exclude="[Microsoft.*]*,[System.*]*,[Scalar.*]*,[*]*Microsoft.AspNetCore.OpenApi.Generated*"
reportgenerator `
    -reports:"**/coverage.cobertura.xml" `
    -targetdir:"coveragereport" `
    -reporttypes:Html `
    -assemblyfilters:"-Microsoft.AspNetCore.OpenApi.Generated;-System.*" `
    -classfilters:"-Microsoft.AspNetCore.OpenApi.Generated;-System.Runtime.CompilerServices*"

Start-Process "coveragereport/index.html"