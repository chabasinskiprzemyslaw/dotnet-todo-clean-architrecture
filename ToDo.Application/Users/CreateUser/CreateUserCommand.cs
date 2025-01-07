using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Users.CreateUser;

public sealed record CreateUserCommand(string FirstName, string LastName, string Email) : ICommand<Guid>;
