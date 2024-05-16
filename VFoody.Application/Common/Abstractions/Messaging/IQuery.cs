using MediatR;
using VFoody.Domain.Shared;

namespace VFoody.Application.Common.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
