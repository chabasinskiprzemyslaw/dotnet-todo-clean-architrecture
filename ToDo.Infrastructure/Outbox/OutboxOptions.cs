﻿namespace ToDo.Infrastructure.Outbox;

internal class OutboxOptions
{
    public int IntervalInSeconds { get; init; }
    public int BatchSize { get; init; }
}
