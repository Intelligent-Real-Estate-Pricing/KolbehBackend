using Application.Cqrs.Behaviors.Models;
using Application.Cqrs.Queris;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Application.Cqrs.Behaviors
{
    public sealed class CachingBehavior<TRequest, TResponse>(IDistributedCache distributedCache, IMemoryCache memoryCache) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IQuery<TResponse>, ICacheableRequest<TResponse>
    where TResponse : class
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheableRequest<TResponse> cacheable)
            {
                if (string.IsNullOrEmpty(cacheable.CacheKey))
                    throw new ArgumentNullException(nameof(request), "null cache key");

                TResponse response = null;
                if (!cacheable.UseMemoryCache)
                {
                    var stringResponse = await distributedCache.GetAsync(cacheable.CacheKey, cancellationToken).ConfigureAwait(false);
                    if (stringResponse is not null)
                    {
                        var stringCache = Encoding.UTF8.GetString(stringResponse);
                        response = stringCache.ToObject<TResponse>();
                    }

                    if (response is not null) return response;

                    response = await next().ConfigureAwait(false);

                    if (response is not null && (cacheable.ConditionFroSetCache is null || cacheable.ConditionFroSetCache(response)))
                        await distributedCache.SetAsync(cacheable.CacheKey, response.ToJsonUtf8Bytes(), new DistributedCacheEntryOptions
                        {
                            AbsoluteExpiration = cacheable.ConditionExpiration(response)
                        }, cancellationToken).ConfigureAwait(false);

                    return response;
                }

                response = memoryCache.Get<TResponse>(cacheable.CacheKey);

                if (response is not null) return response;

                response = await next().ConfigureAwait(false);

                if (response is not null && (cacheable.ConditionFroSetCache is null || cacheable.ConditionFroSetCache(response)))
                    memoryCache.Set(cacheable.CacheKey, response, cacheable.ConditionExpiration(response));

                return response;
            }

            return await next().ConfigureAwait(false);
        }
    }
    static class Extensions
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            PropertyNameCaseInsensitive = true,
            Converters =
        {
            new JsonStringEnumConverter()
        },
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public static T ToObject<T>(this string source)
        {
            return JsonSerializer.Deserialize<T>(source, JsonSerializerOptions);
        }

        public static string ToJson(this object source)
        {
            return JsonSerializer.Serialize(source, JsonSerializerOptions);
        }

        public static byte[] ToJsonUtf8Bytes(this object source)
        {
            return JsonSerializer.SerializeToUtf8Bytes(source, JsonSerializerOptions);
        }
    }
}
