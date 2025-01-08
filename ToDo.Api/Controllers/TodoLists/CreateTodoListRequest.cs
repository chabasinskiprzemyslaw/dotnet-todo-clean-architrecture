using ToDo.Application.Todo.CreateTodoList;

namespace ToDo.Api.Controllers.TodoLists;

public sealed record CreateTodoListRequest(string Title, string? Description)
{
    public CreateTodoListCommand ToCommand() => new CreateTodoListCommand(Title, Description);
}