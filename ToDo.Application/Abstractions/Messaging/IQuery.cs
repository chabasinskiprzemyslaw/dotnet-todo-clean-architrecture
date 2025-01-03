using MediatR;
using ToDo.Domain.Abstractions;

namespace ToDo.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
