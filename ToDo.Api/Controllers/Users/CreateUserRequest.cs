using ToDo.Application.Users.CreateUser;

namespace ToDo.Api.Controllers.Users;

public sealed record CreateUserRequest(string FirstName, string LastName, string Email)
{
    public CreateUserCommand ToCommand() => new(FirstName, LastName, Email);    
}