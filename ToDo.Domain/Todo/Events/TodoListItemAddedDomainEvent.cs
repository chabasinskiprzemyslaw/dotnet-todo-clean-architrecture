using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo.Events;

public sealed record TodoListItemAddedDomainEvent(Guid TodoListId) : IDomainEvent;