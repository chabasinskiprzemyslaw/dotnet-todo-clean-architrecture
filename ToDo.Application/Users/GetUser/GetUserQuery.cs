using ToDo.Application.Abstractions.Messaging;

namespace ToDo.Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;