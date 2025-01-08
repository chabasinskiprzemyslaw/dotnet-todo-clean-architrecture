using ToDo.Domain.Users;

namespace ToDo.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override void Add(User entity)
    {
        foreach (var role in entity.Roles)
        {
            DbContext.Attach(role);
        }

        DbContext.Add(entity);
    }
}
