using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalRequestForAdmin;

public class GetShopWithdrawalRequestQuery : PaginationRequest, IQuery<Result>
{
    public int ShopId { get; set; }
    public string ShopName { get; set; }
    public int Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int OrderBy { get; set; } // 0 desc updated, 1 requested_amount, 2 bankCode DESC
    public int OrderMode { get; set; } // 0 desc, 1 asc
}