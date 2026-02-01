using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using VatCalculator.Shared;

namespace VatCalculator.Api.Tests.IntegrationTests;

[TestFixture]
public class ErrorHandlingTests
{
    private TestWebApplicationFactory _factory;
    private HttpClient _client;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new TestWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task UnhandledException_ReturnsProblemDetails()
    {
        // Act
        var response = await _client.GetAsync(Constants.Throw);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        Assert.That(response.Content.Headers.ContentType?.MediaType, Is.EqualTo("application/problem+json"));

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.Multiple(() =>
        {
            Assert.That(problem, Is.Not.Null);
            Assert.That(problem!.Title, Is.EqualTo("Unexpected error"));
            Assert.That(problem.Status, Is.EqualTo(500));
            Assert.That(problem.Detail, Is.EqualTo("Boom!"));
        });
    }

    [Test]
    public async Task ErrorEndpoint_WithoutExceptionFeature_ReturnsProblemWithoutDetail()
    {
        // Arrange
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/error");

        // Assert
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.That(problem!.Detail, Is.Null);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}