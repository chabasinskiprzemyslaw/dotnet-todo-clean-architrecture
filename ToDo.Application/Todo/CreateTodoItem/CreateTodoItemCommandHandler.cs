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
        Description? description = null;
        var todoList = await _todoListRepository.GetAsync(request.TodoListId, cancellationToken);

        if (todoList is null)
        {
            return Result.Failure<Guid>(TodoListErrors.NotFound);
        }

        Title title = Title.Create(request.Title);

        if (!string.IsNullOrWhiteSpace(request.Description)) 
        {
            description = Description.Create(request.Description);
        }

        TodoItem? todoItem = TodoItem.Create(
            title,
            request.Priority,
            _timeProvider.UtcNow,
            todoList.Id,
            description,
            request.DueDate);

        todoList.AddTodoItem(todoItem);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success<Guid>(todoItem.Id);
    }
}