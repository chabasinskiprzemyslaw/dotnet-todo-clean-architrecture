using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;