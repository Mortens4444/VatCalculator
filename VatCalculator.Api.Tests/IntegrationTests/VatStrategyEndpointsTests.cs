using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using VatCalculator.Shared;

namespace VatCalculator.Api.Tests.IntegrationTests;

[TestFixture]
public class VatStrategyEndpointsTests
{
    private WebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        factory = new WebApplicationFactory<Program>();
        client = factory.CreateClient();
    }

    [Test]
    public async Task Get_Strategies_Returns_All_Strategies()
    {
        // Act
        var response = await client.GetAsync($"{Constants.VatBase}/strategies");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var data = await response.Content.ReadFromJsonAsync<VatStrategyInfo[]>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(data, Is.Not.Null);
            Assert.That(data!, Is.Not.Empty);
            Assert.That(data.Any(s => s.CountryCode == "AT"), Is.True);
        }
    }

    [Test]
    public async Task Get_Strategy_ByCountry_Returns_Strategy()
    {
        // Act
        var response = await client.GetAsync($"{Constants.VatBase}/strategies/AT");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var data = await response.Content.ReadFromJsonAsync<VatStrategyInfo>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(data, Is.Not.Null);
            Assert.That(data!.CountryCode, Is.EqualTo("AT"));
            Assert.That(data.Rates, Is.Not.Empty);
        }
    }

    [Test]
    public async Task Get_Strategy_ByCountry_Unknown_Returns_404()
    {
        // Act
        var response = await client.GetAsync($"{Constants.VatBase}/strategies/XX");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        client.Dispose();
        factory.Dispose();
    }
}