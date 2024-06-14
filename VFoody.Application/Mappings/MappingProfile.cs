using AutoMapper;
using VFoody.Application.UseCases.Accounts.Commands.ForgotPassword;
using VFoody.Application.UseCases.Accounts.Commands.Register;
using VFoody.Application.UseCases.Accounts.Commands.SendCode;
using VFoody.Application.UseCases.Accounts.Commands.VerifyForgotPasswordCode;
using VFoody.Application.UseCases.Accounts.Commands.VerifyRegisterCode;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Application.UseCases.Product.Commands.CreateProductImageOfShopOwner;
using VFoody.Application.UseCases.Product.Commands.CreateProductOfShopOwner;
using VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;

namespace VFoody.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Building, BuildingResponse>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Name));
        CreateMap<Account, AccountResponse>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Building,
                opt => opt.MapFrom(src => src.Building));
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
        CreateMap<Product, ProductCardResponse>();
        CreateMap<CreateProductImageRequest, CreateProductImageCommand>();
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<CreateProductRequest.CreateQuestionRequest, CreateProductCommand.CreateQuestionCommand>();
        CreateMap<CreateProductRequest.CreateOptionRequest, CreateProductCommand.CreateOptionCommand>();
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<UpdateProductRequest.UpdateQuestionRequest, UpdateProductCommand.UpdateQuestionCommand>();
        CreateMap<UpdateProductRequest.UpdateOptionRequest, UpdateProductCommand.UpdateOptionCommand>();
        CreateMap<Product, ProductShopOwnerResponse>();
    }
}
