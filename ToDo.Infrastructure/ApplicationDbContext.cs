using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ToDo.Application.Abstractions.Clock;
using ToDo.Application.Exceptions;
using ToDo.Domain.Abstractions;
using ToDo.Infrastructure.Outbox;

namespace ToDo.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    private readonly IDateTimeProvider _dateTimeProvider;

    public ApplicationDbContext(
        DbContextOptions options,
        IDateTimeProvider dateTimeProvider) : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AddDomainEventsAsOutboxMessages();
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency error", ex);
        }
    }

    public void AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(), 
                _dateTimeProvider.UtcNow, 
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                })))
            .ToList();

        AddRange(outboxMessages);
    }
}
