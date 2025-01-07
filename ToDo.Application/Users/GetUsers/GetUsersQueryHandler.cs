using Dapper;
using ToDo.Application.Abstractions.Data;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;

namespace ToDo.Application.Users.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUsersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<List<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        using var connect = _sqlConnectionFactory.CreateConnection();
        string sql = "SELECT [Id], [FirstName], [LastName], [Email] from Users";
        var result = await connect.QueryAsync<UserResponse>(sql);

        return Result.Success(result.ToList());
    }
}
