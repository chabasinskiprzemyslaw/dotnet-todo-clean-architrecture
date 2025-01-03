using Dapper;
using ToDo.Application.Abstractions.Data;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo;

namespace ToDo.Application.Todo.GetTodoItemsInTodoList;

internal sealed class GetTodoItemsInTodoListQueryHandler : IQueryHandler<GetTodoItemsInTodoListQuery, List<TodoItemResponse>>
{
    private static readonly int[] PriorityStatuses =
    {
        (int)Priority.Low,
        (int)Priority.Medium,
        (int)Priority.High,
    };

    private readonly ISqlConnectionFactory _connectionFactory;

    public GetTodoItemsInTodoListQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<List<TodoItemResponse>>> Handle(GetTodoItemsInTodoListQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = string.Empty;

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
