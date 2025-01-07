using MediatR;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;

namespace ToDo.Application.Users.GetUsers;

public sealed record GetUsersQuery() : IQuery<List<UserResponse>>;