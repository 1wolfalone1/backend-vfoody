using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Product.Commands.CreateProductImageOfShopOwner;
using VFoody.Application.UseCases.Product.Commands.CreateProductOfShopOwner;
using VFoody.Application.UseCases.Product.Commands.UpdateProductStatusOfShopOwner;
using VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;
using VFoody.Application.UseCases.Product.Queries;
using VFoody.Application.UseCases.Product.Queries.CardProducts;
using VFoody.Application.UseCases.Product.Queries.DetailToOrder;
using VFoody.Application.UseCases.Product.Queries.ProductDetailShopOwner;
using VFoody.Application.UseCases.Product.Queries.ProductOfShopOwner;
using VFoody.Application.UseCases.Product.Queries.ShopProduct;
using VFoody.Application.UseCases.Product.Queries.TopProductShop;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class ProductController : BaseApiController
{
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IMapper _mapper;

    public ProductController(IMapper mapper, ICurrentPrincipalService currentPrincipalService)
    {
        _mapper = mapper;
        _currentPrincipalService = currentPrincipalService;
    }

    [HttpGet("customer/product/top")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetTopProduct(int pageIndex, int pageSize)
    {
        return this.HandleResult(await this.Mediator.Send(new GetTopProductQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }

    [HttpGet("customer/product/recent")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetRecentOrderedProductQuery(int pageIndex, int pageSize)
    {
        string email = _currentPrincipalService.CurrentPrincipal;
        return this.HandleResult(await this.Mediator.Send(new GetRecentOrderedProductQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Email = email
        }));
    }

    [HttpGet("shop/{shopId}/product/top")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetTopProductByShop(int shopId, int pageIndex, int pageSize)
    {
        return HandleResult(await Mediator.Send(new GetTopProductShopQuery
        {
            ShopId = shopId,
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }

    [HttpGet("shop/product")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetShopProduct(int shopId, int pageIndex, int pageSize)
    {
        return HandleResult(await Mediator.Send(new GetShopProductQuery
        {
            ShopId = shopId,
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }


    [HttpGet("shop/product/detail")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetProductDetailToOrder(int productId)
    {
        return HandleResult(await Mediator.Send(new GetProductDetailToOrderQuery(productId)));
    }

    [HttpGet("customer/product")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetProductCard([FromQuery] int[] ids)
    {
        return this.HandleResult(await this.Mediator.Send(new GetListProductInCardQuery
        {
            ProductIds = ids
        }));
    }

    [HttpGet("shop-owner/product/detail")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> GetProductDetailShopOwner(int productId)
    {
        return HandleResult(await Mediator.Send(new GetProductDetailQuery(productId, null)));
    }

    [HttpPost("shop-owner/product/image/upload")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> CreateProductImage([FromForm] CreateProductImageRequest createProductImageRequest)
    {
        return HandleResult(await Mediator.Send(_mapper.Map<CreateProductImageCommand>(createProductImageRequest)));
    }

    [HttpPost("shop-owner/product/create")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> CreateProduct(CreateProductRequest createProductRequest)
    {
        return HandleResult(await Mediator.Send(_mapper.Map<CreateProductCommand>(createProductRequest)));
    }

    [HttpGet("shop-owner/product")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> GetProductOfShopOwner(int pageIndex, int pageSize)
    {
        return HandleResult(await Mediator.Send(new GetProductShopOwnerQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            ShopId = null
        }));
    }

    [HttpPut("shop-owner/product/update")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> UpdateProduct(UpdateProductRequest updateProductRequest)
    {
        return HandleResult(await Mediator.Send(_mapper.Map<UpdateProductCommand>(updateProductRequest)));
    }

    [HttpPut("shop-owner/product/status/update")]
    [Authorize(Roles = IdentityConst.ShopClaimName)]
    public async Task<IActionResult> UpdateProductStatus(UpdateProductStatusCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpGet("admin/shop/product")]
    [Authorize(Roles = IdentityConst.AdminClaimName)]
    public async Task<IActionResult> GetProductOfShop(int shopId, int pageIndex, int pageSize)
    {
        return HandleResult(await Mediator.Send(new GetProductShopOwnerQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            ShopId = shopId
        }));
    }

    [HttpGet("admin/product/detail")]
    [Authorize(Roles = IdentityConst.AdminClaimName)]
    public async Task<IActionResult> GetProductDetailShopOwner(int productId, int shopId)
    {
        return HandleResult(await Mediator.Send(new GetProductDetailQuery(productId, shopId)));
    }
}