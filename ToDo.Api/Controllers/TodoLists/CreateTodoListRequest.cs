using ToDo.Application.Todo.CreateTodoList;

namespace ToDo.Api.Controllers.TodoLists;

public sealed record CreateTodoListRequest(Guid OwnerId, string Title, string? Description)
{
    public CreateTodoListCommand ToCommand() => new(OwnerId, Title, Description);
}