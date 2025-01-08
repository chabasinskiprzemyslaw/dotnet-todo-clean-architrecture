using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Todo.CreateTodoList;

public sealed record CreateTodoListCommand(
    string Title,
    string? Description) : ICommand<Guid>;
