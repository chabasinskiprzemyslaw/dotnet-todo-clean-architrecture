using ToDo.Application.Abstractions.Clock;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo;
using ToDo.Domain.Users;

namespace ToDo.Application.Todo.CreateTodoItem;

internal sealed class CreateTodoItemCommandHandler : ICommandHandler<CreateTodoItemCommand, Guid>
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _timeProvider;

    public CreateTodoItemCommandHandler(
        ITodoListRepository todoListRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _todoListRepository = todoListRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        TodoList? todoList = await _todoListRepository.GetAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
        {
            return Result.Failure<Guid>(TodoListErrors.NotFound);
        }

        var todoItemResult = todoList.CreateTodoItem(
            Title.Create(request.Title),
            request.Priority,
            _timeProvider.UtcNow,
            Description.Create(request.Description ?? string.Empty),
            request.DueDate);

        if (todoItemResult.IsFailure)
        {
            return Result.Failure<Guid>(todoItemResult.Error);
        }

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch(Exception ex)
        {
            throw;
        }


        return Result.Success(todoItemResult.Value.Id);
    }
}