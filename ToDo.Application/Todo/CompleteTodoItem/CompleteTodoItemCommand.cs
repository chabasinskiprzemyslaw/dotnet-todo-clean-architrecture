using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Todo.CompleteTodoItem;

public sealed record CompleteTodoItemCommand(Guid TodoListId, Guid TodoItemId) : ICommand;