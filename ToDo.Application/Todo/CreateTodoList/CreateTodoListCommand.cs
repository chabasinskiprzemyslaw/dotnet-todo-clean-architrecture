using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Todo.CreateTodoList;

public sealed record CreateTodoListCommand(
    Guid OwnerId,
    string Title,
    string? Description) : ICommand<Guid>;
