using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using VatCalculator.Shared;

namespace VatCalculator.Api.Tests.IntegrationTests;

[TestFixture]
public class ProgramTests
{
    //[Test]
    //public void Startup_MissingApiVersion_ThrowsException()
    //{
    //    // Üres konfigurációval kényszerítjük ki a hibát
    //    var builder = WebApplication.CreateBuilder();
    //    builder.Configuration["Api:Version"] = null;

    //    Assert.Throws<ArgumentNullException>(() => {
    //        // Itt az API indítási logikáját hívjuk meg
    //        // Vagy használhatsz WebApplicationFactory-t Custom konfigurációval
    //    });
    //}

    [TestCase("Test", HttpStatusCode.InternalServerError)]
    [TestCase("Production", HttpStatusCode.NotFound)] // ErrorHandlingEndpoints.cs env.IsEnvironment("Test") is false, so this endpoint is not created
    //[TestCase("Production", HttpStatusCode.InternalServerError)] // ErrorHandlingEndpoints.cs env.IsEnvironment("Test") is commented out, to test
    public async Task Startup_InProduction_UsesExceptionHandler(string environment, HttpStatusCode expectedHttpStatusCode)
    {
        using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(environment);
            });

        var client = factory.CreateClient();

        var response = await client.GetAsync(Constants.Throw);
        Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
    }
}
