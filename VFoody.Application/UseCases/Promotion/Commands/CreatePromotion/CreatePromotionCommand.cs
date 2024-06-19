using FluentValidation;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.CreatePromotion;

public class CreatePromotionCommand : ICommand<Result>
{
    public CreatePromotionRequest CreatePromotion { get; set; }
}

public class Validator : AbstractValidator<CreatePromotionCommand>
{
    public Validator()
    {
        RuleFor(x => x.CreatePromotion).SetValidator(new CreatePromotionValidate());
    }
}