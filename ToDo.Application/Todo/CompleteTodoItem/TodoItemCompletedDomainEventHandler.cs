using MediatR;
using ToDo.Application.Abstractions.Email;
using ToDo.Domain.Todo;
using ToDo.Domain.Todo.Events;
using ToDo.Domain.Users;

namespace ToDo.Application.Todo.CompleteTodoItem;

internal sealed class TodoItemCompletedDomainEventHandler : INotificationHandler<TodoItemCompletedDomainEvent>
{
    //NOTE: Repository approach
    private readonly IEmailService _emailService;
    private readonly ITodoListRepository _todoListRepository;
    private readonly IUserRepository _userRepository;

    public TodoItemCompletedDomainEventHandler(IEmailService emailService, ITodoListRepository todoListRepository, IUserRepository userRepository)
    {
        _emailService = emailService;
        _todoListRepository = todoListRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(TodoItemCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var todoList = await _todoListRepository.GetAsync(notification.TodoListId, cancellationToken);

        if (todoList is null)
        {
            return;
        }

        var todoItem = todoList.TodoItems.FirstOrDefault(x => x.Id == notification.TodoListId);

        if (todoItem is null)
        {
            return;
        }

        var user = await _userRepository.GetAsync(todoList.OwnerId, cancellationToken);

        if (user is null)
        {
            return;
        }

        await _emailService.SendAsync(user.Email, "You made it!", $"You completed task {todoItem.Title}");
    }
}
