using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Queries.AccountInfo;

public sealed record GetAccountInfoQuery(int accountId) : IQuery<Result>;