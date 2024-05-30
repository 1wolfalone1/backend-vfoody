using AutoMapper;
using VFoody.Application.UseCases.Accounts.Commands;
using VFoody.Application.UseCases.Accounts.Commands.ForgotPassword;
using VFoody.Application.UseCases.Accounts.Commands.SendCode;
using VFoody.Application.UseCases.Accounts.Commands.Verify;
using VFoody.Application.UseCases.Accounts.Commands.VerifyForgotPasswordCode;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;

namespace VFoody.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountResponse>();
        CreateMap<CustomerRegisterRequest, CustomerRegisterCommand>();
        CreateMap<ForgotPasswordRequest, ForgotPasswordCommand>();
        CreateMap<VerifyCodeForgotPasswordRequest, VerifyCodeForgotPasswordCommand>();
        CreateMap<AccountVerifyRequest, AccountVerifyCommand>();
        CreateMap<AccountSendCodeRequest, AccountSendCodeCommand>();
        CreateMap<Product, ProductDetailResponse>();
        CreateMap<Question, ProductDetailResponse.QuestionResponse>();
        CreateMap<Option, ProductDetailResponse.OptionResponse>();
        CreateMap<Product, ProductResponse>();
        CreateMap<Shop, ShopInfoResponse>()
            .ForMember(dest => dest.Rating,
                opt => opt.MapFrom(src => src.TotalRating == 0 ? 0 : Math.Round((double)src.TotalStar / src.TotalRating, 1)));
        CreateMap<Building, ShopInfoResponse.BuildingResponse>();
    }
}
