using Microsoft.AspNetCore.Http;
using VFoody.Domain.Enums;

namespace VFoody.Application.UseCases.Feedbacks.Commands.CustomerCreateFeedback;

public class CustomerCreateFeedbackRequest
{
    public RatingRanges Rating { get; set; }
    public string Comment { get; set; }
    public IFormFile[] Images { get; set; }
}