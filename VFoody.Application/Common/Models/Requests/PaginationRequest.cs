using VFoody.Application.Common.Constants;

namespace VFoody.Application.Common.Models.Requests;

public abstract class PaginationRequest
{

    public int PageIndex { get; set; } = PaginationConstants.DefaultPageNumber;
    public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;
}
