﻿namespace ToDo.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public DateTime OccurredOnUtc { get; init; }
    public string Type { get; init; }
    public string Content { get; init; }
    public DateTime? ProcessedOnUtc { get; init; }
    public string? Error { get; init; }

    public OutboxMessage(
        Guid id,
        DateTime occurredOnUtc,
        string type,
        string content)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
        Type = type;
        Content = content;
    }
}
