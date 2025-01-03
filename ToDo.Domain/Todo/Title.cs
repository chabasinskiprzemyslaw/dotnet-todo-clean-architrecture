namespace ToDo.Domain.Todo;

public sealed record Title
{
    public string Value { get; }

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Title cannot be empty");
        }

        Value = value;
    }
}
