using FluentValidation;
using VFoody.Domain.Enums;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalHistory;

public class GetShopWithdrawalHistoryValidator : AbstractValidator<GetShopWithdrawalHistoryQuery>
{
    public GetShopWithdrawalHistoryValidator()
    {
        RuleFor(x => x.Status)
            .Must(BeAValidTransactionType)
            .WithMessage("Vui lòng cung cấp status từ 1 (Pending), 2 (Approved), 3 (Reject)")
            .When(x => x.Status != 0);
    }
    private bool BeAValidTransactionType(int status)
    {
        return Enum.IsDefined(typeof(ShopWithdrawalRequestStatus), status);
    }
}