using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using VatCalculator.Shared;

namespace VatCalculator.Api.Tests.IntegrationTests;

[TestFixture]
public class VatCalculationEndpointsTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task Post_CalculateFromNet_ReturnsSuccess()
    {
        // Arrange
        // Net: 100 + 20% VAT = 120 Gross
        var request = new VatCalculationFromNetRequest { Net = 100, VatRate = 20, CountryCode = "AT" };

        // Act
        // Feltételezve: Constants.VatCalculateEndpoint + "/from-net" vagy Constants.VatCalculateFromNetEndpoint
        var response = await _client.PostAsJsonAsync(Constants.VatCalculateFromNetEndpoint, request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await response.Content.ReadFromJsonAsync<VatCalculationResponse>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(data?.Net, Is.EqualTo(100));
            Assert.That(data?.VatAmount, Is.EqualTo(20));
            Assert.That(data?.Gross, Is.EqualTo(120));
        }
    }

    [Test]
    public async Task Post_CalculateFromGross_ReturnsSuccess()
    {
        // Arrange
        // Gross: 120 (20%) -> Net: 100
        var request = new VatCalculationFromGrossRequest { Gross = 120, VatRate = 20, CountryCode = "AT" };

        // Act
        var response = await _client.PostAsJsonAsync(Constants.VatCalculateFromGrossEndpoint, request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await response.Content.ReadFromJsonAsync<VatCalculationResponse>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(data?.Gross, Is.EqualTo(120));
            Assert.That(data?.Net, Is.EqualTo(100)); // 120 / 1.2
            Assert.That(data?.VatAmount, Is.EqualTo(20));
        }
    }

    [Test]
    public async Task Post_CalculateFromVat_ReturnsSuccess()
    {
        // Arrange
        // VAT: 20 (20%) -> Net: 100, Gross: 120
        var request = new VatCalculationFromVatRequest { VatAmount = 20, VatRate = 20, CountryCode = "AT" };

        // Act
        var response = await _client.PostAsJsonAsync(Constants.VatCalculateFromVatEndpoint, request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await response.Content.ReadFromJsonAsync<VatCalculationResponse>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(data?.VatAmount, Is.EqualTo(20));
            Assert.That(data?.Net, Is.EqualTo(100)); // 20 / 0.2
            Assert.That(data?.Gross, Is.EqualTo(120));
        }
    }

    [Test]
    public async Task Post_InvalidCountry_ReturnsBadRequest()
    {
        // Arrange: "XYZ" nem létező országkód
        var request = new VatCalculationFromNetRequest { Net = 100, VatRate = 20, CountryCode = "XYZ" };

        // Act
        var response = await _client.PostAsJsonAsync(Constants.VatCalculateFromNetEndpoint, request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}