using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Entities;

namespace VFoody.Application.UseCases.Accounts.Queries;

public class GetAllAccounQuery :  IQuery<List<Account>>
{
}
