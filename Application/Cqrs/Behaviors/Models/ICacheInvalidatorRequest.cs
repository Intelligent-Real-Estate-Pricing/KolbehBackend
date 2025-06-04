// Ignore Spelling: Cacheable Invalidator

namespace Application.Cqrs.Behaviors.Models
{
    public interface ICacheInvalidatorRequest
    {
        string CacheKey { get; }
    }
}
