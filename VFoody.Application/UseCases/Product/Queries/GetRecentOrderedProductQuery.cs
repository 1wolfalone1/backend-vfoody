using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries;

public class GetRecentOrderedProductQuery :  PaginationRequest, IQuery<Result>
{
    public string Email { get; set; }
}
