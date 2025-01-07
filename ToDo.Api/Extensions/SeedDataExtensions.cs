using Bogus;
using Dapper;
using ToDo.Application.Abstractions.Data;
using ToDo.Domain.Users;

namespace ToDo.Api.Extensions;

public static  class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var sqlConnection = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using var connection = sqlConnection.CreateConnection();

        var faker = new Faker();

        List<object> users = new();

        for (var i = 0; i < 5; i++)
        {
            users.Add(new {
                Id = Guid.NewGuid(),
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                Email = faker.Internet.Email()
            });
        }

        const string sql = """
            INSERT INTO Users (Id, FirstName, LastName, Email)
            VALUES (@Id, @FirstName, @LastName, @Email)
            """;

        connection.Execute(sql, users);
    }
}
