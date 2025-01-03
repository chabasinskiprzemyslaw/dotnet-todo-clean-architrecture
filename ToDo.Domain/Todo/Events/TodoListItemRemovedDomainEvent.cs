﻿using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo.Events;

public sealed record TodoListItemRemovedDomainEvent(Guid TodoListId) : IDomainEvent;