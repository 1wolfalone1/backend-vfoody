using System.Text.Json.Serialization;
using Net.payOS;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Domain.Enums;
using VFoody.Infrastructure.Common.Data.ApplicationInitialData;
using VFoody.Infrastructure.Persistence.Repositories;
using VFoody.Infrastructure.Services;
using VFoody.Infrastructure.Services.Dapper;

namespace VFoody.API.Extensions;

public static class InfrastructureServiceExtension
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        //Set enum request
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonNumericEnumConverter());
            });
        
        // Initial data
        services.AddScoped<ApplicationDbInitializer>();
        using var scope = services.BuildServiceProvider().CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>();
        if (IsDevelopment())
        {
            initializer.SeedAsync().Wait();
        }
        // add PayOs
        PayOS payos = new PayOS(configuration["PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
            configuration["PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
            configuration["PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));
        services.AddSingleton(payos);
        
        return services;
    }
    
    private static bool IsDevelopment()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return environment == Environments.Development;
    }
}