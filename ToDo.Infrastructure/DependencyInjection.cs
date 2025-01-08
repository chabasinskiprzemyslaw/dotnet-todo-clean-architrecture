using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using ToDo.Application.Abstractions.Authentication;
using ToDo.Application.Abstractions.Caching;
using ToDo.Application.Abstractions.Clock;
using ToDo.Application.Abstractions.Data;
using ToDo.Application.Abstractions.Email;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo;
using ToDo.Domain.Users;
using ToDo.Infrastructure.Authentication;
using ToDo.Infrastructure.Authorization;
using ToDo.Infrastructure.Caching;
using ToDo.Infrastructure.Clock;
using ToDo.Infrastructure.Data;
using ToDo.Infrastructure.Email;
using ToDo.Infrastructure.Outbox;
using ToDo.Infrastructure.Repositories;
namespace ToDo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();
        AddPersistance(services, configuration);
        AddAuthentication(services, configuration);
        AddAuthorization(services);
        AddCaching(services, configuration);
        AddHealthChecks(services, configuration);
        AddApiVersioning(services);
        AddBackgroundJobs(services, configuration);

        return services;
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();

        services.Configure<ToDo.Infrastructure.Authentication.AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<ToDo.Application.Abstractions.Authentication.IAuthenticationService, ToDo.Infrastructure.Authentication.AuthenticationService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
        })
            .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();
    }

    private static void AddPersistance(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodoListRepository, TodoListRepository>();

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
    }

    private static void AddAuthorization(this IServiceCollection services)
    {
        services.AddScoped<AuthorizationService>();
        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Cache") ?? 
            throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration) 
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!)
            .AddRedis(configuration.GetConnectionString("Cache")!)
            .AddUrlGroup(new Uri(configuration["KeyCloak:BaseUrl"]!), HttpMethod.Get, "keycloak");
    }

    private static void AddApiVersioning(IServiceCollection services)
    {
        services.AddApiVersioning(o =>
        {
            o.DefaultApiVersion = new ApiVersion(1);
            o.ReportApiVersions = true;
            //o.ApiVersionReader = new HeaderApiVersionReader();
            //o.ApiVersionReader = new QueryStringApiVersionReader();
            //o.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader(), new UrlSegmentApiVersionReader());
            o.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
            .AddMvc()
            .AddApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'V";
                o.SubstituteApiVersionInUrl = true;
            });
    }

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("Outbox");
        services.Configure<OutboxOptions>(config);

        services.AddQuartz();

        services.AddQuartzHostedService(o => o.WaitForJobsToComplete = true);
        services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
    }
}
