namespace ToDo.Domain.Todo;

public sealed record Title
{
    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Title Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Title cannot be empty");
        }

        return new Title(value);
    }
}
