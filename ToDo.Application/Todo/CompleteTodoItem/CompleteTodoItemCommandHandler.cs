using ToDo.Application.Abstractions.Clock;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo;

namespace ToDo.Application.Todo.CompleteTodoItem;

internal sealed class CompleteTodoItemCommandHandler : ICommandHandler<CompleteTodoItemCommand>
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _timeProvider;

    public CompleteTodoItemCommandHandler(
        ITodoListRepository todoListRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider timeProvider
        )
    {
        _todoListRepository = todoListRepository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _todoListRepository.GetAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
        {
            return Result.Failure(TodoListErrors.NotFound);
        }

        var todoItem = todoList.TodoItems.FirstOrDefault(x => x.Id == request.TodoListId);

        if (todoItem is null)
        {
            return Result.Failure(TodoItemErrors.NotFound);
        }

        todoItem.MarkAsCompleted(_timeProvider.UtcNow);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
