﻿using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<bool> CheckInOrderByProductId(int id);
}
