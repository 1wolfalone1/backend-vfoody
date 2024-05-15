using FluentValidation.AspNetCore;
using FluentValidation;
using VFoody.Application.Mappings;
using System.Data;
using MySql.Data.MySqlClient;
using MediatR;
using VFoody.Application.Behaviors;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Infrastructure.Persistence.Repositories;
using VFoody.Infrastructure.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Infrastructure.Services.Dapper;

namespace VFoody.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //Add memory cache
        services.AddMemoryCache();

        //Dapper
        services.AddScoped<IDbConnection>((sp) => new MySqlConnection(config["DATABASE_URL"]));

        //Allow origin
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", poli =>
            {
                poli.AllowAnyMethod().AllowAnyHeader().WithOrigins(config["ALLOW_ORIGIN"]);

            });
        });

        // MediaR
        var applicationAssembly = typeof(Application.AssemblyReference).Assembly;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Auto mapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // FluentAPI validation
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }

    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        //Config service

        services.Scan(scan => scan
        .FromAssemblyOf<IAccountService>() // Application Layer
        .AddClasses(classes => classes.AssignableTo<AccountService>())
        .AsImplementedInterfaces()
        .WithTransientLifetime()
        .FromAssemblyOf<AccountRepository>() // Infrastructure Layer
        .AddClasses(classes => classes.AssignableTo<IAccountRepository>())
        .AsImplementedInterfaces()
        .WithTransientLifetime());

        //Add unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDapperService, DapperService>();
        return services;
    }
}
