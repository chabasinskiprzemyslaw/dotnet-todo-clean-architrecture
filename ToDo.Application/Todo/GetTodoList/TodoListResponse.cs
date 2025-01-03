namespace ToDo.Application.Todo.GetTodoList;

//Primitive types and flat structure for easily return from database
public sealed class TodoListResponse
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public DateOnly CreatedOnUtc { get; init; }
}