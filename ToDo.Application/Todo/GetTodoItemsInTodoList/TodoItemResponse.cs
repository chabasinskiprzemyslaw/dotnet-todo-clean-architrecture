namespace ToDo.Application.Todo.GetTodoItemsInTodoList;

public sealed class TodoItemResponse
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }
}