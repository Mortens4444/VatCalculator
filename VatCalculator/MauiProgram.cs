using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VatCalculator;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        var apiSection = builder.Configuration.GetSection("Api");
        builder.Services.Configure<ApiOptions>(apiSection);
        
        var apiOptions = apiSection.Get<ApiOptions>();
        var baseUrl = apiOptions?.BaseUrl ?? "http://localhost:5210";
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });

        var app = builder.Build();

        return app;
    }
}
