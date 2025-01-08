using Dapper;
using ToDo.Application.Abstractions.Data;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Application.Todo.GetTodoList;
using ToDo.Domain.Abstractions;


namespace ToDo.Application.Todo.GetTodoLists;

internal sealed class GetTodoListsQueryHandler : IQueryHandler<GetTodoListsQuery, List<TodoListResponse>>
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetTodoListsQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<List<TodoListResponse>>> Handle(GetTodoListsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = "SELECT [Id], [Title], [Description], [OwnerId], [CreatedOnUtc] FROM TodoLists";

        var todoLists = await connection.QueryAsync<TodoListResponse>(sql);

        return Result.Success(todoLists.ToList());
    }
}