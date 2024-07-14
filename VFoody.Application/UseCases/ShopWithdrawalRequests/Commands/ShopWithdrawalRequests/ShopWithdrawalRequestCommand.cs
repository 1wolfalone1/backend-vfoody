using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.ShopWithdrawalRequests;

public class ShopWithdrawalRequestCommand : ICommand<Result>
{
    public ShopWithdrawalCommandRequest CommandRequestModel { get; set; }
}

public class ShopWithdrawalRequestCommandValidator : AbstractValidator<ShopWithdrawalRequestCommand>
{
    public ShopWithdrawalRequestCommandValidator()
    {
        RuleFor(x => x.CommandRequestModel).SetValidator(new WithdrawalRequestValidator());
    }
}