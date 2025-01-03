using System.Diagnostics.CodeAnalysis;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo.Events;

namespace ToDo.Domain.Todo;

public sealed class TodoItem : Entity
{
    public Title Title { get; private set; }
    public Description? Description { get; private set; }
    public DateTime? DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public Priority Priority { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }

    private TodoItem(
        Guid id,
        Title title,
        Description? description,
        DateTime? dueDate,
        Priority priority,
        DateTime nowUtc) : base(id)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        CreatedOnUtc = nowUtc;
    }

    public static TodoItem Create(
        Title title,
        Priority priority,
        DateTime createdOnUtc,
        Description? description = null,
        DateTime? dueDate = null)
    {
        if (dueDate is not null && dueDate < DateTime.UtcNow)
        {
            throw new ArgumentException("Due date cannot be in the past", nameof(dueDate));
        }

        TodoItem todoItem = new TodoItem(
            Guid.NewGuid(),
            title,
            description,
            dueDate,
            priority,
            createdOnUtc);

        todoItem.RaiseDomainEvent(new TodoItemCreatedDomainEvent(todoItem.Id));

        return todoItem;
    }

    public Result MarkAsCompleted(DateTime completedOnUtc)
    {
        if (IsCompleted)
        {
            return Result.Failure(TodoItemErrors.AlreadyCompleted);
        }

        IsCompleted = true;
        CompletedOnUtc = completedOnUtc;

        RaiseDomainEvent(new TodoItemCompletedDomainEvent(Id));

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
