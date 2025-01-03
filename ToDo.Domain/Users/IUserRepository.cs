namespace ToDo.Domain.Users;

public interface IUserRepository
{
    Task<User> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}
