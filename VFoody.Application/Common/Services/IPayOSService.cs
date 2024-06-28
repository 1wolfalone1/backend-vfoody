using VFoody.Application.UseCases.Orders.Models;
using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Services;

public interface IPayOSService
{
    Task<CreatePayment> CheckOut(List<ItemPayment> items, int orderCode,
        float paymentAmount, Order order, string cancelUrl, string successUrl);
}