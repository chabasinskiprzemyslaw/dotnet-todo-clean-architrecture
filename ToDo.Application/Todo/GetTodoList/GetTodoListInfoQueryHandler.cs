using Dapper;
using ToDo.Application.Abstractions.Data;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;

namespace ToDo.Application.Todo.GetTodoList;

internal sealed class GetTodoListInfoQueryHandler : IQueryHandler<GetTodoListInfoQuery, TodoListResponse?>
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetTodoListInfoQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<TodoListResponse?>> Handle(GetTodoListInfoQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = "SELECT [Id], [Title], [Description], [OwnerId], [CreatedOnUtc] FROM TodoLists";

        TodoListResponse? todoList = await connection.QueryFirstOrDefaultAsync<TodoListResponse>(
            sql,
            new
            {
                request.TodoListId
            });

        return Result.Success(todoList);
    }
}