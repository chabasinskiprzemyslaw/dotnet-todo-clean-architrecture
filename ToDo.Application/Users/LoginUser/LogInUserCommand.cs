using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Users.LoginUser;

public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;
