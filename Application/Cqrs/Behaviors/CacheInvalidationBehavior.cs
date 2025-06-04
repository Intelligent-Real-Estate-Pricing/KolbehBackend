using Application.Cqrs.Behaviors.Models;
using Application.Cqrs.Commands;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Cqrs.Behaviors
{
    public sealed class CacheInvalidationBehavior<TRequest, TResponse>(IDistributedCache distributedCache) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>, ICacheInvalidatorRequest
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheInvalidatorRequest invalidator)
            {
                if (string.IsNullOrEmpty(invalidator.CacheKey))
                    throw new ArgumentNullException(nameof(request), "null cache key");

                var response = await next().ConfigureAwait(false);

                await distributedCache.RemoveAsync(invalidator.CacheKey, cancellationToken).ConfigureAwait(false);

                return response;
            }

            return await next().ConfigureAwait(false);
        }
    }
}
