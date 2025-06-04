// Ignore Spelling: Cacheable

using Application.Cqrs.Behaviors.Models;

namespace Application.Cqrs.Queris;

public interface ICacheableQuery<TResult> : IQuery<TResult>, ICacheableRequest<TResult> where TResult : class
{

}
