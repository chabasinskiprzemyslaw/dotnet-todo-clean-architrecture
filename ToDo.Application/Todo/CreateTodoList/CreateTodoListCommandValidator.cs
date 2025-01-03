using FluentValidation;

namespace ToDo.Application.Todo.CreateTodoList;

public sealed class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}
