namespace ToDo.Domain.Todo;

public sealed record Description
{
    public string Value { get; set; }

    private Description(string value)
    {
        Value = value;
    }

    public static Description Create(string value)
    {
        return new Description(value);
    }
}
