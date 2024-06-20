using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Queries.ManageAccount;

public class GetAllAccountQuery : PaginationRequest, IQuery<Result>
{
}
