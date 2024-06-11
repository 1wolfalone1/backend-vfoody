using FluentValidation;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Dashboard.Queries.Overview;

public class GetDashboardAdminOverviewQuery : IQuery<Result>
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; } = DateTime.Now;
}

public class Validate : AbstractValidator<GetDashboardAdminOverviewQuery>
{
    public Validate()
    {
        RuleFor(x => x.DateFrom)
            .LessThanOrEqualTo(x => x.DateTo)
            .WithMessage("DateFrom phải nhỏ hơn Date to");
        RuleFor(x => x.DateTo)
            .GreaterThanOrEqualTo(x => x.DateFrom)
            .WithMessage("DateTo phải lớn hơn Date From");
    }
}