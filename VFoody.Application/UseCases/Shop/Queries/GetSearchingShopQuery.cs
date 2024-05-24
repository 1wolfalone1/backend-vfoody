using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries;

public class GetSearchingShopQuery :  IQuery<Result>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string SearchText { get; set; }
    public int OrderType { get; set; }
    public int CurrentBuildingId { get; set; }
}
