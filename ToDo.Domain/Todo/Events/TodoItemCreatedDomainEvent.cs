using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo.Events;

public sealed record TodoItemCreatedDomainEvent(
    Guid TodoItemId) : IDomainEvent;