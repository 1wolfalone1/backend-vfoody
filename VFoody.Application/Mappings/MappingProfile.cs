using AutoMapper;
using VFoody.Application.UseCases.Accounts.Commands;
using VFoody.Application.UseCases.Accounts.Commands.SendCode;
using VFoody.Application.UseCases.Accounts.Commands.Verify;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Entities;

namespace VFoody.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountResponse>();
        CreateMap<CustomerRegisterRequest, CustomerRegisterCommand>();
        CreateMap<AccountVerifyRequest, AccountVerifyCommand>();
        CreateMap<AccountSendCodeRequest, AccountSendCodeCommand>();
        CreateMap<Product, ProductDetailResponse>();
        CreateMap<Question, ProductDetailResponse.QuestionResponse>();
        CreateMap<Option, ProductDetailResponse.OptionResponse>();
        CreateMap<Product, ProductResponse>();
    }
}
