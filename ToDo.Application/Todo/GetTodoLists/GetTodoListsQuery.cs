using ToDo.Application.Abstractions.Messaging;
using ToDo.Application.Todo.GetTodoList;

namespace ToDo.Application.Todo.GetTodoLists;

public sealed record GetTodoListsQuery() : IQuery<List<TodoListResponse>>;
