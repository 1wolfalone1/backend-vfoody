using AutoMapper;
using VFoody.Application.UseCases.Accounts.Commands.ForgotPassword;
using VFoody.Application.UseCases.Accounts.Commands.Register;
using VFoody.Application.UseCases.Accounts.Commands.SendCode;
using VFoody.Application.UseCases.Accounts.Commands.VerifyForgotPasswordCode;
using VFoody.Application.UseCases.Accounts.Commands.VerifyRegisterCode;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Application.UseCases.Notifications.Models;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Application.UseCases.Product.Commands.CreateProductImageOfShopOwner;
using VFoody.Application.UseCases.Product.Commands.CreateProductOfShopOwner;
using VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Application.UseCases.Promotion.Commands.UpdatePromotionInfo;
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;

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
                opt => opt.MapFrom(src =>
                    src.TotalRating == 0 ? 0 : Math.Round((double)src.TotalStar / src.TotalRating, 1)));
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
        CreateMap<Account, ManageAccountResponse>()
            .ForMember(
                dest => dest.Role,
                opt => opt.MapFrom(
                    src =>
                        src.RoleId == (int)Roles.Customer ? "Khách hàng" :
                        src.RoleId == (int)Roles.Shop ? "Chủ cửa hàng" :
                        string.Empty
                )
            ).ForMember(dest => dest.FullName,
                opt =>
                    opt.MapFrom(src => src.LastName != string.Empty ? src.LastName : src.Email)
            )
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(
                    src =>
                        src.Status == (int)AccountStatus.UnVerify ? "Chưa xác thực" :
                        src.Status == (int)AccountStatus.Verify ? "Đang hoạt động" :
                        src.Status == (int)AccountStatus.Ban ? "Đã bị cấm" :
                        string.Empty
                )
            );
        CreateMap<ManageShopDto, ManageShopResponse>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(
                    src =>
                        src.Status == (int)ShopStatus.Active ? "Đã phê duyệt" :
                        src.Status == (int)ShopStatus.UnActive ? "Chưa phê duyệt" :
                        src.Status == (int)ShopStatus.Ban ? "Đã bị cấm" :
                        string.Empty
                )
            ).ForMember(dest => dest.Active,
                opt => opt.MapFrom(
                    src =>
                        src.Active == 1 ? "Đang hoạt động" :
                        src.Active == 0 ? "Đang đóng cửa" :
                        string.Empty
                )
            )
            ;
        CreateMap<ManageOrderDto, ManageOrderResponse>().ForMember(dest => dest.Status,
            opt => opt.MapFrom(
                src =>
                    src.Status == (int)OrderStatus.Pending ? "Đang thực hiện" :
                    src.Status == (int)OrderStatus.Confirmed ? "Đang thực hiện" :
                    src.Status == (int)OrderStatus.Delivering ? "Đang thực hiện" :
                    src.Status == (int)OrderStatus.Successful ? "Đã hoàn thành" :
                    src.Status == (int)OrderStatus.Cancelled ? "Đã hủy" :
                    src.Status == (int)OrderStatus.Fail ? "Giao không thành công" :
                    src.Status == (int)OrderStatus.Rejected ? "Đã hủy" :
                    string.Empty
            )
        );

        CreateMap<ManageShopDetailDto, ManageShopDetailResponse>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(
                    src =>
                        src.Status == (int)ShopStatus.Active ? "Đã phê duyệt" :
                        src.Status == (int)ShopStatus.UnActive ? "Chưa phê duyệt" :
                        src.Status == (int)ShopStatus.Ban ? "Đã bị cấm" :
                        string.Empty
                )
            ).ForMember(dest => dest.Active,
                opt => opt.MapFrom(
                    src =>
                        src.Active == 1 ? "Đang hoạt động" :
                        src.Active == 0 ? "Đang đóng cửa" :
                        string.Empty
                )
            )
            ;
        CreateMap<Account, AccountInfoResponse>().ForMember(dest => dest.FullName,
            opt =>
                opt.MapFrom(src => src.LastName != string.Empty ? src.LastName : src.Email));
        CreateMap<Notification, NotificationResponse>();
        CreateMap<PlatformPromotion, AllPromotionResponse>();
        CreateMap<PersonPromotion, AllPromotionResponse>();
        CreateMap<ShopPromotion, AllPromotionResponse>();
    }
}