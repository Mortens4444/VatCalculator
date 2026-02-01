# VatCalculator

A multi-platform VAT (Value Added Tax) calculation application with a RESTful API backend and Blazor frontend.

## 🎯 Features

- Calculate VAT for multiple countries (Austria, Hungary)
- Support for different VAT rates per country
- Calculate from Net, Gross, or VAT amount
- RESTful API with OpenAPI/Swagger documentation
- Interactive API documentation with Swagger UI and Scalar
- Blazor-based user interface
- Comprehensive unit tests with code coverage

## 🏗️ Project Structure

```
VatCalculator/
├── VatCalculator.Api/              # ASP.NET Core Web API
│   ├── Endpoints/                  # API endpoint definitions
│   ├── Services/                   # VAT calculation strategies
│   ├── Interfaces/                 # Service interfaces
│   └── Extensions/                 # Extension methods and configuration
├── VatCalculator.Api.Tests/        # NUnit test project
├── VatCalculator.Shared/           # Shared DTOs and models
└── VatCalculator/                  # Blazor frontend (optional)
```

## 🚀 Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- Visual Studio 2026, Visual Studio Code, or JetBrains Rider

### Running the API

1. Clone the repository
```bash
git clone <repository-url>
cd VatCalculator
```

2. Navigate to the API project
```bash
cd VatCalculator.Api
```

3. Run the application
```bash
dotnet run
```

4. Access the API documentation:
   - Swagger UI: `https://localhost:7297/swagger`
   - Scalar UI: `https://localhost:7297/scalar/v1`
   - OpenAPI JSON: `https://localhost:7297/openapi/v1.json`
   - OpenAPI YAML: `https://localhost:7297/openapi/v1.yaml`

### Running Tests

```bash
cd VatCalculator.Api.Tests
dotnet test
```

## 📊 Code Coverage

### Setup Coverage Tools

1. Install ReportGenerator global tool:
```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

### Generate Coverage Report

1. Run tests with coverage:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

2. Generate HTML report:
```bash
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

3. Open the report:
```bash
# Windows
start coveragereport/index.html

# macOS
open coveragereport/index.html

# Linux
xdg-open coveragereport/index.html
```

### Automated Coverage Script

The included `CodeCoverage.ps1` or `CodeCoverage.sh` script automates the entire process

Make the script executable:
```bash
chmod +x CodeCoverage.sh
./CodeCoverage.sh
```

## 🔧 API Usage

### Calculate VAT from Net Amount

```bash
POST /api/vat/calculate
Content-Type: application/json

{
  "countryCode": "AT",
  "vatRate": 20,
  "net": 100
}
```

Response:
```json
{
  "net": 100.00,
  "vatAmount": 20.00,
  "gross": 120.00
}
```

### Calculate VAT from Gross Amount

```bash
POST /api/vat/calculate
Content-Type: application/json

{
  "countryCode": "HU",
  "vatRate": 27,
  "gross": 127
}
```

### Calculate VAT from VAT Amount

```bash
POST /api/vat/calculate
Content-Type: application/json

{
  "countryCode": "AT",
  "vatRate": 20,
  "vatAmount": 20
}
```

## 🌍 Supported Countries and VAT Rates

| Country | Code | Standard Rate | Reduced Rates |
|---------|------|---------------|---------------|
| Austria | AT   | 20%           | 10%, 13%      |
| Hungary | HU   | 27%           | 5%, 18%       |

## 🧪 Testing

The project includes comprehensive unit tests covering:

- Valid VAT rate validation
- Multiple input validation (only one input allowed)
- Missing input validation
- Calculation accuracy from Net amount
- Calculation accuracy from Gross amount
- Calculation accuracy from VAT amount