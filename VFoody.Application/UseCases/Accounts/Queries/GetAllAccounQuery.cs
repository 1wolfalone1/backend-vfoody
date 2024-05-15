using MediatR;
using VFoody.Domain.Entities;

namespace VFoody.Application.UseCases.Accounts.Queries;

public class GetAllAccounQuery : IRequest<List<Account>> 
{
}
