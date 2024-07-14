using FluentValidation;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalRequestForAdmin;

public class GetShopWithdrawalRequestValidator : AbstractValidator<GetShopWithdrawalRequestQuery>
{
    public GetShopWithdrawalRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status != 0)
            .WithMessage("Vui lòng cung cấp status từ 1 (Pending), 2 (Approved), 3 (Reject)");
    }
}