using Dapper;
using ToDo.Application.Abstractions.Authentication;
using ToDo.Application.Abstractions.Data;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo;

namespace ToDo.Application.Todo.GetTodoList;

internal sealed class GetTodoListInfoQueryHandler : IQueryHandler<GetTodoListInfoQuery, TodoListResponse?>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly IUserContext _userContext;

    public GetTodoListInfoQueryHandler(
        ISqlConnectionFactory connectionFactory,
        IUserContext userContext)
    {
        _connectionFactory = connectionFactory;
        _userContext = userContext;
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

        if (todoList is null || todoList.OwnerId != _userContext.UserId)
        {
            return Result.Failure<TodoListResponse?>(TodoListErrors.NotFound);
        }

        return Result.Success(todoList);
    }
}