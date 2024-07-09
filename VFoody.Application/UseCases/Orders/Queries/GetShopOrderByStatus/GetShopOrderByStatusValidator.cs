using FluentValidation;

namespace VFoody.Application.UseCases.Orders.Queries.GetShopOrderByStatus;

public class GetShopOrderByStatusValidator : AbstractValidator<GetShopOrderByStatusQuery>
{
    public GetShopOrderByStatusValidator()
    {
        RuleForEach(x => x.Status)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(7)
            .WithMessage("Status phải nằm trong khoảng từ 1 - 7");
    }
}