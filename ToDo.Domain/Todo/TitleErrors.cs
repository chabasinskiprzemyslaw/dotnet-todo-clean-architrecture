using ToDo.Domain.Abstractions;

namespace ToDo.Domain.Todo;

public static class TitleErrors
{
    public static Error Empty => new Error("Title.Empty", "Title cannot be empty");
}
