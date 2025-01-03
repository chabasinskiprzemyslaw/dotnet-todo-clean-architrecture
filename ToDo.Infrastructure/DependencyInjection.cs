using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Abstractions.Clock;
using ToDo.Application.Abstractions.Data;
using ToDo.Application.Abstractions.Email;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo;
using ToDo.Domain.Users;
using ToDo.Infrastructure.Clock;
using ToDo.Infrastructure.Data;
using ToDo.Infrastructure.Email;
using ToDo.Infrastructure.Repositories;

namespace ToDo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodoListRepository, TodoListRepository>();

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        return services;
    }
}
