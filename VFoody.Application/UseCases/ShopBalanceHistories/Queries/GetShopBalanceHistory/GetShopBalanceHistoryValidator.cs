using FluentValidation;
using VFoody.Domain.Enums;

namespace VFoody.Application.UseCases.ShopBalanceHistories.Queries.GetShopBalanceHistory;

public class GetShopBalanceHistoryValidator : AbstractValidator<GetShopBalanceHistoryQuery>
{
    public GetShopBalanceHistoryValidator()
    {
        RuleFor(x => x.TransactionType)
            .Must(BeAValidTransactionType)
            .WithMessage("Vui lòng cung cấp status từ 1 (Order Payment), 2 (Refund), 3 (Manual Adjustment)")
            .When(x => x.TransactionType != 0);
    }
    private bool BeAValidTransactionType(int status)
    {
        return Enum.IsDefined(typeof(ShopBalanceTransactionTypes), status);
    }
}