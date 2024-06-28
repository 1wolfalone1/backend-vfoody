using Net.payOS;
using Net.payOS.Types;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Services;

public class PayOsService : BaseService, IPayOSService
{
    private readonly PayOS _payOS;

    public PayOsService(PayOS payOs)
    {
        _payOS = payOs;
    }

    public async Task<CreatePayment> CheckOut(List<ItemPayment> items, int orderCode,
        float paymentAmount, Order order, string cancelUrl, string successUrl)
    {
        List<ItemData> itemDatas = items.Select(x => new ItemData(
            x.Name, x.Quantity, x.Price)).ToList();
        int amount = (int)paymentAmount;
        PaymentData paymentData = new PaymentData(orderCode, amount, $"VFD{order.Id}", itemDatas,
            cancelUrl, successUrl, null, order.FullName, string.Empty,
            order.PhoneNumber, order.Building.Name);
        CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
        var result = new CreatePayment()
        {
            Bin = createPayment.bin,
            AccountNumber = createPayment.accountNumber,
            Amount = createPayment.amount,
            Description = createPayment.description,
            OrderCode = createPayment.orderCode,
            Currency = createPayment.currency,
            PaymentLinkId = createPayment.paymentLinkId,
            Status = createPayment.status,
            CheckoutUrl = createPayment.checkoutUrl,
            QrCode = createPayment.qrCode
        };

        return result;
    }
}