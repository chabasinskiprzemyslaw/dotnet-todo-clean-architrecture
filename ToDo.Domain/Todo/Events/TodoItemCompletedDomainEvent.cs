using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo.Events;

public sealed record TodoItemCompletedDomainEvent(Guid TodoItemId) : IDomainEvent;