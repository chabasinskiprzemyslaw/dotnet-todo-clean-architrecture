using ToDo.Application.Abstractions.Clock;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo;
using ToDo.Domain.Users;

namespace ToDo.Application.Todo.CreateTodoList;

internal sealed class CreateTodoListCommandHandler : ICommandHandler<CreateTodoListCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly ITodoListRepository _todoListRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateTodoListCommandHandler(
            IUserRepository userRepository,
            ITodoListRepository todoListRepository,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _todoListRepository = todoListRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        string descritpionString = request.Description ?? string.Empty;
        var user = await _userRepository.GetAsync(request.OwnerId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        Title title = Title.Create(request.Title);

        Description description = Description.Create(descritpionString);

        var todoList = TodoList.Create(title, user.Id, _dateTimeProvider.UtcNow, description);

        _todoListRepository.Add(todoList);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success<Guid>(todoList.Id);
    }
}
