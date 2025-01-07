using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Todo.GetTodoItemsInTodoList;

public sealed record GetTodoListItemsQuery(Guid TodoListId) : IQuery<List<TodoItemResponse>>;
