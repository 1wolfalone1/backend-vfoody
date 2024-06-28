using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.FavouriteShop.Commands.FavouriteShop;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class FavouriteShopController : BaseApiController
{
    [HttpPut("customer/favourite/shop")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> FavouriteShop([FromBody] FavouriteShopCommand favouriteShopCommand)
    {
        return HandleResult(await Mediator.Send(favouriteShopCommand));
    }
}