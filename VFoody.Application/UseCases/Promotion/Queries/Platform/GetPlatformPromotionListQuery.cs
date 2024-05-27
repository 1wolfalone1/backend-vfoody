﻿using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.Platform;

public class GetPlatformPromotionListQuery : IQuery<Result>
{
    public int Status { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public bool Available { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}