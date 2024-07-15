using FluentValidation;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalRequestForAdmin;

public class GetShopWithdrawalRequestValidator : AbstractValidator<GetShopWithdrawalRequestQuery>
{
    public GetShopWithdrawalRequestValidator()
    {
        RuleFor(x => x.DateFrom)
            .LessThan(x => x.DateTo)
            .When(x => x.DateFrom.HasValue && x.DateTo.HasValue)
            .WithMessage("Ngày bắt đầu phải nhỏ hơn Ngày kết thúc khi cả hai đều không null.");
        
        RuleFor(x => x.OrderBy)
            .InclusiveBetween(0, 2)
            .WithMessage("OrderBy phải nằm trong khoảng từ 0 đến 2.");
        
        RuleFor(x => x.OrderMode)
            .InclusiveBetween(0, 1)
            .WithMessage("OrderMode phải nằm trong khoảng từ 0 đến 1.");
    }
}