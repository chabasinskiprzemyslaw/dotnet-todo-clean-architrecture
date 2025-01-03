namespace ToDo.Domain.Todo;

public interface ITodoListRepository
{
    Task<TodoList> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TodoList>> GetAllAsync();
    Task AddAsync(TodoList TodoList);
    Task UpdateAsync(TodoList TodoList);
    Task DeleteAsync(TodoList TodoList);
}
