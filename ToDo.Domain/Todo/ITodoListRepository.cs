namespace ToDo.Domain.Todo;

public interface ITodoListRepository
{
    Task<TodoList?> GetAsync(Guid id, CancellationToken cancellationToken);
    void Add(TodoList TodoList);
}
