using Microsoft.Extensions.Options;
using Quartz;

namespace ToDo.Infrastructure.Outbox;

internal class ProcessOutboxMessagesJobSetup : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions;

    public ProcessOutboxMessagesJobSetup(IOptions<OutboxOptions> outboxOptions)
    {
        _outboxOptions = outboxOptions.Value;
    }

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(ProcessOutboxMessagesJob);

        options.AddJob<ProcessOutboxMessagesJob>(c => c.WithIdentity(jobName))
            .AddTrigger(config => config
            .ForJob(jobName)
            .WithSimpleSchedule(
                schedule => schedule
                .WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));
    }
}
