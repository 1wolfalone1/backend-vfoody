using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Transactions.Commands.PaymentForOrder.PaymentSuccess;

public class PaymentSucessCommand : ICommand<Result>
{
    public int Code { get; set; }
    public string Id { get; set; }
    public bool Cancel { get; set; }
    public string Status { get; set; }
    public int OrderCode { get; set; }
    public int OrderId { get; set; }
}