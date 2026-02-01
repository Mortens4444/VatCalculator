using Scalar.AspNetCore;
using VatCalculator.Api.Interfaces;

namespace VatCalculator.Api.Extensions;

public static class VatStrategyServiceRegister
{
    public static IServiceCollection AddVatStrategyServices(this IServiceCollection services)
    {
        var strategyType = typeof(IVatStrategy);

        var implementations = strategyType.Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && strategyType.IsAssignableFrom(t));

        foreach (var implementation in implementations)
        {
            services.AddSingleton(strategyType, implementation);
        }

        return services;
    }
}
