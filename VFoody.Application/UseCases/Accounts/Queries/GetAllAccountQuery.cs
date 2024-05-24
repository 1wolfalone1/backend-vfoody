using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Queries;

public class GetAllAccountQuery :  IQuery<Result<List<Account>>>
{
}
