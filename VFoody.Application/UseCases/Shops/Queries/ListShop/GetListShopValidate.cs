using FluentValidation;

namespace VFoody.Application.UseCases.Shop.Queries.ListShop;

public class GetListShopValidate : AbstractValidator<GetListShopQuery>
{
    public GetListShopValidate()
    {
        RuleFor(x => x.shopIds)
            .NotEmpty()
            .WithMessage("Vui lòng truyền danh sách id");
    }
}