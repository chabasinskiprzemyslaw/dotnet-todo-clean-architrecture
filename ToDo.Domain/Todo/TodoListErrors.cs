using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo;

public static class TodoListErrors
{
    public static Error TodoItemNotFound => new Error("TodoList.TodoItemNotFound", "Todo item not found");
}
