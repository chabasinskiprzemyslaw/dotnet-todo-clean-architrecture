using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo;

public static class TodoItemErrors
{
    public static Error DueDateInThePast => new Error("TodoItem.DueDateInThePast", "Due date cannot be in the past");
    public static Error AlreadyCompleted => new Error("TodoItem.AlreadyCompleted", "Todo item is already completed");
}
