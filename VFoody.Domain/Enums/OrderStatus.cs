using System.ComponentModel;

namespace VFoody.Domain.Enums;

public enum OrderStatus
{
    [Description("Placed: Your order has been successfully placed.")] OrderPLaced = 1,
    [Description("The restaurant has confirmed your order")] OrderConfirmed = 2,
    [Description("The restaurant is preparing your food.")] Preparing = 3,
    [Description("Your order is on its way.")] OutForDelivery = 4,
    [Description("Your order has been delivered. Enjoy your meal!")] Delivered = 5,
    [Description("our order has been cancelled.")] Cancelled = 6,
    [Description("Your order has been refunded.")] Refunded = 7
}