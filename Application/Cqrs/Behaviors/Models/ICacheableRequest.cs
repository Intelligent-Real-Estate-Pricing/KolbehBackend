// Ignore Spelling: Cacheable Invalidator

using System.Text.Json.Serialization;

namespace Application.Cqrs.Behaviors.Models
{
    public interface ICacheableRequest<T> where T : class
    {
        string CacheKey { get; }

        [JsonIgnore]
        Func<T, DateTimeOffset> ConditionExpiration { get; }

        bool UseMemoryCache { get; }

        [JsonIgnore]
        virtual Func<T, bool> ConditionFroSetCache => null;

    }
}
