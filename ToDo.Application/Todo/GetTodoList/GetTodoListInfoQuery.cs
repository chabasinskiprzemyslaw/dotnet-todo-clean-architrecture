using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Todo.GetTodoList;

public sealed record GetTodoListInfoQuery(Guid TodoListId) : IQuery<TodoListResponse?>;
