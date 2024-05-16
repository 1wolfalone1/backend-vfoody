using MediatR;
using VFoody.Domain.Shared;

namespace VFoody.Application.Common.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}