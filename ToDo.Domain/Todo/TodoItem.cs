using System.Diagnostics.CodeAnalysis;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo.Events;

namespace ToDo.Domain.Todo;

public sealed class TodoItem : Entity
{
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public DateTime? DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public Priority Priority { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public Guid TodoListId { get; private set; }

    private TodoItem() {}

    private TodoItem(
        Guid id,
        Guid todoListId,
        Title title,
        Description description,
        DateTime? dueDate,
        Priority priority,
        DateTime nowUtc) : base(id)
    {
        TodoListId = todoListId;
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        CreatedOnUtc = nowUtc;
    }

    public static Result<TodoItem> Create(
        Title title,
        Priority priority,
        DateTime createdOnUtc,
        Guid todoListId,
        Description description = null,
        DateTime? dueDate = null)
    {
        if (dueDate is not null && dueDate < DateTime.UtcNow)
        {
            return Result.Failure<TodoItem>(TodoItemErrors.DueDateInThePast);
        }

        TodoItem todoItem = new TodoItem(
            Guid.NewGuid(),
            todoListId,
            title,
            description,
            dueDate,
            priority,
            createdOnUtc);

        todoItem.RaiseDomainEvent(new TodoItemCreatedDomainEvent(todoItem.Id));

        return Result.Success(todoItem);
    }

    public Result MarkAsCompleted(DateTime completedOnUtc)
    {
        if (IsCompleted)
        {
            return Result.Failure(TodoItemErrors.AlreadyCompleted);
        }

        IsCompleted = true;
        CompletedOnUtc = completedOnUtc;

        RaiseDomainEvent(new TodoItemCompletedDomainEvent(Id, TodoListId));

        return Result.Success();
    }

    public Result ChangePriority(Priority priority)
    {
        Priority = priority;

        RaiseDomainEvent(new TodoItemChangedPriorityDomainEvent(Id));

        return Result.Success();
    }

    public Result ChangeDueDate(DateTime? dueDate)
    {
        if (dueDate is not null && dueDate < DateTime.UtcNow)
        {
            return Result.Failure(TodoItemErrors.DueDateInThePast);
        }

        DueDate = dueDate;

        return Result.Success();
    }
}
