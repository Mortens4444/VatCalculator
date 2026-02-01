using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Net.Http.Json;
using VatCalculator.Api.Interfaces;
using VatCalculator.Api.Tests.TestStrategies;
using VatCalculator.Shared;

namespace VatCalculator.Api.Tests.IntegrationTests;

[TestFixture]
public class VatCalculationEndpointErrorTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IVatStrategy>();
                services.AddSingleton<IVatStrategy, MaliciousStrategy>();
            });
        });
        _client = _factory.CreateClient();
    }

    // NUnit1032 javítása: Az IDisposable objektumok felszabadítása
    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task FromNet_WhenArgumentException_ReturnsBadRequest()
    {
        var request = new VatCalculationFromNetRequest { Net = 100, VatRate = 27, CountryCode = "ERR" };
        var response = await _client.PostAsJsonAsync($"{Constants.VatBase}/calculate/from-net", request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Does.Contain("Test invalid rate message"));
    }

    [Test]
    public async Task FromGross_WhenGeneralException_ReturnsInternalServerError()
    {
        var request = new VatCalculationFromGrossRequest { Gross = 100, VatRate = 27, CountryCode = "ERR" };
        var response = await _client.PostAsJsonAsync($"{Constants.VatBase}/calculate/from-gross", request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Does.Contain("An unexpected error occurred."));

#if DEBUG
        Assert.That(content, Does.Contain("Unexpected system failure"));
#endif
    }
}
