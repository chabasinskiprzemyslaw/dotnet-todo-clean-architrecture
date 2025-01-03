using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo.Events;

public sealed record TodoItemChangedPriorityDomainEvent(Guid TodoItemId) : IDomainEvent;