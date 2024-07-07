using System.Windows.Input;
using FluentValidation;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Feedbacks.Commands.CustomerCreateFeedback;

public class CustomerCreateFeedbackCommand : ICommand<Result>
{
    public CustomerCreateFeedbackRequest RequestModel { get; set; }
    public int OrderId { get; set; }
}

public class Validator : AbstractValidator<CustomerCreateFeedbackCommand>
{
    public Validator()
    {
        RuleFor(x => x.RequestModel).SetValidator(new CreateFeedbackValidator());
    }
}