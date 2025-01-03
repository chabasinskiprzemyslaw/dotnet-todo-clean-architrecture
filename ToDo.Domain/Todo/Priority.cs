namespace ToDo.Domain.Todo;

public sealed record Priority
{
    private static readonly Priority Low = new("LOW");
    private static readonly Priority Medium = new("MEDIUM");
    private static readonly Priority High = new("HIGH");

    private Priority(string value) => Value = value;
    public string Value { get; set; }

    public static Priority From(string value) => value.ToUpperInvariant() switch
    {
        "LOW" => Low,
        "MEDIUM" => Medium,
        "HIGH" => High,
        _ => throw new ArgumentException("Invalid priority value", nameof(value))
    };

    public static readonly IReadOnlyCollection<Priority> All = new[] { Low, Medium, High };
}
