using ToDo.Domain.Todo;

namespace ToDo.Api.Controllers.TodoLists;

public sealed record CreateTodoItemRequest(
    string Title,
    Priority Priority,
    string? Description,
    DateTime? DueDate);