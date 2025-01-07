using MediatR;
using ToDo.Domain.Abstractions;

namespace ToDo.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : 
    IRequestHandler<TCommand, Result> where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
