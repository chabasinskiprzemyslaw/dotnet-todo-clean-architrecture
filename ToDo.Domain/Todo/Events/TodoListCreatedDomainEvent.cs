﻿using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo.Events;

public sealed record TodoListCreatedDomainEvent(Guid TodoListId) : IDomainEvent;