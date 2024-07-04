using FluentValidation;
using VFoody.Domain.Enums;

namespace VFoody.Application.UseCases.Product.Queries.ProductOfShopOwner;

public class GetProductShopOwnerValidate : AbstractValidator<GetProductShopOwnerQuery>
{
    public GetProductShopOwnerValidate()
    {
        When(x => x.Status.HasValue, () =>
        {
            RuleFor(x => x.Status)
                .Must(status => status == (int)ProductStatus.Active || status == (int)ProductStatus.UnActive)
                .WithMessage("Trạng thái sản phẩm chỉ có thể là 1 (Active) hoặc 2 (InActive).");
        });
    }
}