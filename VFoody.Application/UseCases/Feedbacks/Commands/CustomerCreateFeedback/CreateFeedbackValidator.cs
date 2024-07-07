using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace VFoody.Application.UseCases.Feedbacks.Commands.CustomerCreateFeedback;

public class CreateFeedbackValidator : AbstractValidator<CustomerCreateFeedbackRequest>
{
    public CreateFeedbackValidator()
    {
        RuleFor(x => x.Rating)
            .IsInEnum()
            .WithMessage("Vui lòng đánh giá cho đơn hàng từ 1 - 5 sao");

        RuleForEach(x => x.Images)
            .Must(BeAValidSize)
            .WithMessage("File ảnh phải nhỏ hơn 100Mb")
            .When(x => x.Images != default && x.Images.Length > 0);
        
        RuleForEach(x => x.Images)
            .Must(BeAValidFileType)
            .WithMessage("Vui lòng cung ảnh đúng định dạng")
            .When(x => x.Images != default && x.Images.Length > 0);
    }

    private bool BeAValidSize(IFormFile file)
    {
        const int maxFileSizeInBytes = 100 * 1024 * 1024; // 100 MB
        return file.Length <= maxFileSizeInBytes;
    }
    
    private bool BeAValidFileType(IFormFile file)
    {
        string[] validTypes = { "image/jpeg", "image/jpg", "image/png" };
        return validTypes.Contains(file.ContentType);
    }
}