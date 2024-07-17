using FluentValidation;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.ShopWithdrawalRequests;

public class WithdrawalRequestValidator : AbstractValidator<ShopWithdrawalCommandRequest>
{
    public WithdrawalRequestValidator()
    {
        RuleFor(x => x.BankShortName)
            .NotEmpty().WithMessage("Tên viết tắt của ngân hàng không được để trống.");

        RuleFor(x => x.BankAccountNumber)
            .NotEmpty().WithMessage("Số tài khoản ngân hàng không được để trống.");

        RuleFor(x => x.RequestedAmount)
            .GreaterThan(1000).WithMessage("Số tiền yêu cầu phải lớn hơn 10.000 đ.");

    }
}