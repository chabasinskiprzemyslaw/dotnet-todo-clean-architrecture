using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Quartz;
using System.Data;
using ToDo.Application.Abstractions.Clock;
using ToDo.Application.Abstractions.Data;
using ToDo.Domain.Abstractions;

namespace ToDo.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Beginning to process outbox messages");

        using var connection = _sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);
        foreach (var outboxMessage in outboxMessages) 
        { 
            Exception? exception = null;

            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    JsonSerializerSettings
                    )!;

                await _publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception ex) 
            {
                _logger.LogError(
                    ex,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);
            }

            transaction.Commit();

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);

            _logger.LogInformation("Completed processing outbox messages");
        }

    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(IDbConnection connection, IDbTransaction transaction)
    {
        var sql = $@"
            SELECT TOP {_outboxOptions.BatchSize} Id, Content FROM OutboxMessages
            WHERE ProcessedOnUtc IS NULL
            ORDER BY OccurredOnUtc";

        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse message,
        Exception? exception)
    {

        const string sql = @"UPDATE OutboxMessages SET ProcessedOnUtc = @ProcessedOnUtc, Error = @Error WHERE Id = @Id";

        await connection.ExecuteAsync(sql,
            new
            {
                Id = message.Id,
                ProcessedOnUtc = _dateTimeProvider.UtcNow,
                Error = exception?.ToString(),
            }, transaction: transaction);
    }

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}
