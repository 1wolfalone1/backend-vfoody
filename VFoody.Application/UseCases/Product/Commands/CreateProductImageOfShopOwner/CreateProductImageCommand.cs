using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.CreateProductImageOfShopOwner;

public sealed record CreateProductImageCommand(IFormFile File) : ICommand<Result>;