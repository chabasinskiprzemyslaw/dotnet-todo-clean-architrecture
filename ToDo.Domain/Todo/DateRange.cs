namespace ToDo.Domain.Todo;

public sealed record DateRange
{
    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }

    private DateRange() { }
    private DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }

    public static DateRange Create(DateOnly start, DateOnly end)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        return new DateRange(start, end);
    }
}
