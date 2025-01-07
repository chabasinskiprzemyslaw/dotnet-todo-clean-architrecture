using ToDo.Application.Users.RegisterUser;

namespace ToDo.Api.Controllers.Users;

public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password)
{
    public RegisterUserCommand ToCommand() => new RegisterUserCommand(Email, FirstName, LastName, Password);
}
