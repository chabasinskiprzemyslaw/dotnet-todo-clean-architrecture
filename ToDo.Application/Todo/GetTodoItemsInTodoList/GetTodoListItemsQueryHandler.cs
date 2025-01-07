using Dapper;
using ToDo.Application.Abstractions.Data;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo;

namespace ToDo.Application.Todo.GetTodoItemsInTodoList;

internal sealed class GetTodoListItemsQueryHandler : IQueryHandler<GetTodoListItemsQuery, List<TodoItemResponse>>
{
    private static readonly int[] PriorityStatuses =
    {
        (int)Priority.Low,
        (int)Priority.Medium,
        (int)Priority.High,
    };

    private readonly ISqlConnectionFactory _connectionFactory;

    public GetTodoListItemsQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<List<TodoItemResponse>>> Handle(GetTodoListItemsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = "SELECT [Id], [Title], [Description], [IsCompleted] from TodoItems";

        var todoItems = await connection.QueryAsync<TodoItemResponse>(
            sql,
            new
            {
                request.TodoListId,
                PriorityStatuses
            });

        return Result.Success(todoItems.ToList());
    }
}
