using Microsoft.Extensions.Caching.Distributed;

namespace ToDo.Infrastructure.Caching;

public static class CacheOptions
{
    public static DistributedCacheEntryOptions Default => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
    };

    public static DistributedCacheEntryOptions Create(TimeSpan? exp) => exp is not null ?
        new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = exp } :
        Default;
}