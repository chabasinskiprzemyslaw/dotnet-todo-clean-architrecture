using Dapper;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using ToDo.Api.Extensions;
using ToDo.Application;
using ToDo.Application.Abstractions.Data;
using ToDo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((
    hostingContext, loggerConfiguration) => 
    loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//builder.Services.AddHealthChecks()
//    .AddCheck<CustomSqlHealthCheck>("custom-sql");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();

    //app.SeedData();
}

app.UseHttpsRedirection();

app.UserRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Configure the health check endpoint for the UI
app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

//Custom health check
//public class CustomSqlHealthCheck(ISqlConnectionFactory sqlConnectionFactory) : IHealthCheck
//{
//    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            using var connection = sqlConnectionFactory.CreateConnection();

//            await connection.ExecuteScalarAsync("SELECT 1;");

//            return HealthCheckResult.Healthy();
//        }
//        catch (Exception ex) 
//        {
//            return HealthCheckResult.Unhealthy(exception: ex);
//        }

//    }
//}
