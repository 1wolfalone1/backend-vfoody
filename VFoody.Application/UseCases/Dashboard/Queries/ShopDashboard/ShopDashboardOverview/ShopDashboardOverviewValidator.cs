using FluentValidation;

namespace VFoody.Application.UseCases.Dashboard.Queries.ShopDashboard.ShopDashboardOverview;

public class ShopDashboardOverviewValidator : AbstractValidator<ShopDashboardOverviewQuery>
{
    public ShopDashboardOverviewValidator()
    {
        RuleFor(x => x.DateTo)
            .LessThanOrEqualTo(x => x.DateTo)
            .WithMessage("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
    }
}