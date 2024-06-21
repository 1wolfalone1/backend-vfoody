using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.FavouriteShop.Commands.FavouriteShop;

public sealed record FavouriteShopCommand(int ShopId) : ICommand<Result>;