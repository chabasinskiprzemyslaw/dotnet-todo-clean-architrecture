using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Todo;

namespace ToDo.Application.Todo.CreateTodoItem;

public sealed record CreateTodoItemCommand(
    Guid TodoListId,
    string Title,
    Priority Priority,
    string? Description,
    DateTime? DueDate) : ICommand<Guid>;
