using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Todo.GetTodoItemsInTodoList;

public sealed record GetTodoItemsInTodoListQuery(Guid TodoListId) : IQuery<List<TodoItemResponse>>;
