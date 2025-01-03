using ToDo.Domain.Todo;

namespace ToDo.Infrastructure.Repositories;

internal sealed class TodoListRepository : Repository<TodoList>, ITodoListRepository
{
    public TodoListRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}