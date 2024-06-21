using FluentValidation;

namespace VFoody.Application.UseCases.Orders.Queries.GetOrderByStatusOfCustomer;

public class GetOrderByStatusOfCustomerValidate : AbstractValidator<GetOrderByStatusOfCustomerQuery>
{
    public GetOrderByStatusOfCustomerValidate()
    {
        RuleFor(x => x.Status)
            .NotNull()
            .WithMessage("Trạng thái cửa hàng không thể trống");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Id của tài khoản không thể thiếu");
    }
}