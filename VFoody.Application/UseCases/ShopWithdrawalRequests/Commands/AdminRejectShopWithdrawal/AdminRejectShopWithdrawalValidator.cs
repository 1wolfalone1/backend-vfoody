using FluentValidation;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.AdminRejectShopWithdrawal;

public class AdminRejectShopWithdrawalValidator : AbstractValidator<AdminRejectShopWithdrawalCommand>
{
    public AdminRejectShopWithdrawalValidator()
    {
        RuleFor(x => x.ShopId)
            .GreaterThan(0)
            .WithMessage("ShopId không thể trống và phải lớn hơn 0.");

        RuleFor(x => x.RequestId)
            .GreaterThan(0)
            .WithMessage("RequestId không thể trống và phải lớn hơn 0.");
        
        RuleFor(x => x.Reason)
            .NotNull()
            .NotEmpty()
            .Must(x => x.Length < 1000)
            .WithMessage("Lý do không thể trống và ngắn hơn 1000 ký tự");
    }
}