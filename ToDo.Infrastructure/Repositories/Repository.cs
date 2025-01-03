using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Abstractions;

namespace ToDo.Infrastructure.Repositories;

internal abstract class Repository<T> where T : Entity
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext
            .Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        DbContext
            .Set<T>()
            .Add(entity);
    }
}
