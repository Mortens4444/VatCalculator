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
public class VatStrategyEndpointErrorTests
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
                services.AddSingleton<IVatStrategy, ExplosiveStrategy>();
            });
        });
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetStrategies_WhenExceptionOccurs_ReturnsInternalServerError()
    {
        // Act
        var response = await _client.GetAsync($"{Constants.VatBase}/strategies");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        var problem = await response.Content.ReadFromJsonAsync<Microsoft.AspNetCore.Mvc.ProblemDetails>();
        Assert.That(problem?.Title, Is.EqualTo("Failed to fetch VAT strategies"));
    }

    [Test]
    public async Task GetStrategyByCountry_WhenExceptionOccurs_ReturnsInternalServerError()
    {
        // Act
        var response = await _client.GetAsync($"{Constants.VatBase}/strategies/HU");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        var problem = await response.Content.ReadFromJsonAsync<Microsoft.AspNetCore.Mvc.ProblemDetails>();
        Assert.That(problem?.Title, Is.EqualTo("Failed to fetch VAT strategy"));
    }
}