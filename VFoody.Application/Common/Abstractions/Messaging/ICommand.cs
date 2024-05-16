using MediatR;
using VFoody.Domain.Shared;

namespace VFoody.Application.Common.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
