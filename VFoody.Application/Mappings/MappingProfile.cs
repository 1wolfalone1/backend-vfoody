using AutoMapper;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;

namespace VFoody.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountResponse>();
    }
}
