using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo.Events;

public sealed record TodoListRenamedDomainEvent(Guid TodoListId) : IDomainEvent;