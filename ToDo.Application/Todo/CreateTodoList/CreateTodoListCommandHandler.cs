using ToDo.Application.Abstractions.Authentication;
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
    private readonly IUserContext _userContext;

    public CreateTodoListCommandHandler(
            IUserRepository userRepository,
            ITodoListRepository todoListRepository,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IUserContext userContext)
    {
        _userRepository = userRepository;
        _todoListRepository = todoListRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
    }

    public async Task<Result<Guid>> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        string descritpionString = request.Description ?? string.Empty;
        Guid userId = _userContext.UserId;
        var user = await _userRepository.GetAsync(userId, cancellationToken);

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
