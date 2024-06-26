﻿using System.Security.Claims;

namespace VFoody.Application.Common.Services;

public interface ICurrentPrincipalService
{
    public string? CurrentPrincipal { get; }

    public int? CurrentPrincipalId { get; }

    public ClaimsPrincipal GetCurrentPrincipalFromToken(string token);
}