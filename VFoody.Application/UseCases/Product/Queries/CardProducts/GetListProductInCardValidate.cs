using FluentValidation;

namespace VFoody.Application.UseCases.Product.Queries.CardProducts;

public class GetListProductInCardValidate : AbstractValidator<GetListProductInCardQuery>
{
    public GetListProductInCardValidate()
    {
        RuleFor(x => x.ProductIds)
            .NotEmpty()
            .WithMessage("Vui lòng truyền danh sách id");
    }
}