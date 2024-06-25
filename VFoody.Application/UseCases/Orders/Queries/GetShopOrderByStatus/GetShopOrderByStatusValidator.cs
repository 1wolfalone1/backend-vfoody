using FluentValidation;

namespace VFoody.Application.UseCases.Orders.Queries.GetShopOrderByStatus;

public class GetShopOrderByStatusValidator : AbstractValidator<GetShopOrderByStatusQuery>
{
    public GetShopOrderByStatusValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status phải nằm trong khoảng từ 1 - 7");
    }
}