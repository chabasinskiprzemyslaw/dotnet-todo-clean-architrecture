using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;
