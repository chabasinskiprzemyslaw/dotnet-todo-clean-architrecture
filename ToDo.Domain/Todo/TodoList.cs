using ToDo.Domain.Abstractions;
using ToDo.Domain.Todo.Events;

namespace ToDo.Domain.Todo;

public sealed class TodoList : Entity
{
    private TodoList(
        Guid id,
        Title title,
        Guid ownerId,
        DateTime utcNow,
        Description? description = null) : base(id)
    {
        Title = title;
        OwnerId = ownerId;
        Description = description;
        CreatedOnUtc = utcNow;
    }

    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Guid OwnerId { get; private set; }
    public List<TodoItem> TodoItems { get; private set; } = new();
    public DateTime CreatedOnUtc { get; private set; }

    public static TodoList Create(
        Title title,
        Guid ownerId,
        DateTime utcNow,
        Description description)
    {
        TodoList todoList = new TodoList(Guid.NewGuid(), title, ownerId, utcNow, description);

        todoList.RaiseDomainEvent(new TodoListCreatedDomainEvent(todoList.Id));

        return todoList;
    }

    public void AddTodoItem(TodoItem todoItem)
    {
        TodoItems.Add(todoItem);
        RaiseDomainEvent(new TodoListItemAddedDomainEvent(Id));
    }

    public Result RemoveTodoItem(Guid todoItemId)
    {
        TodoItem todoItem = TodoItems.FirstOrDefault(x => x.Id == todoItemId);

        if (todoItem is null)
        {
            return Result.Failure(TodoListErrors.TodoItemNotFound);
        }

        TodoItems.Remove(todoItem);

        RaiseDomainEvent(new TodoListItemRemovedDomainEvent(Id));

        return Result.Success();
    }

    public void RenameList(Title title)
    {
        Title = title;

        RaiseDomainEvent(new TodoListRenamedDomainEvent(Id));
    }
}
