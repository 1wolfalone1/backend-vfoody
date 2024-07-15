using FluentValidation;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.AdminApproveShopWithdrawal;

public class AdminApproveShopWithdrawalValidator : AbstractValidator<AdminApproveShopWithdrawalCommand>
{
    public AdminApproveShopWithdrawalValidator()
    {
        RuleFor(x => x.ShopId)
            .GreaterThan(0)
            .WithMessage("ShopId không thể trống và phải lớn hơn 0.");

        RuleFor(x => x.RequestId)
            .GreaterThan(0)
            .WithMessage("RequestId không thể trống và phải lớn hơn 0.");
    }
}