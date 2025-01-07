using Dapper;
using ToDo.Application.Abstractions.Data;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Users;

namespace ToDo.Application.Users.GetUser;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "SELECT * FROM Users WHERE Id = @Id";
        var user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, new { Id = request.UserId });

        return user is not null
            ? Result.Success(user)
            : Result.Failure<UserResponse>(UserErrors.NotFound);
    }
}