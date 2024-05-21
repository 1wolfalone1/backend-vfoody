namespace ArtHubBO.Payload;

public class PageInfo
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public PageInfo() { }
    public PageInfo(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public PageInfo(int pageIndex, int pageSize, int totalItems)
        : this(pageIndex, pageSize)
    {
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
    }

    public PageInfo(PageInfo pageInfo)
    {
        if (pageInfo != null)
        {
            PageIndex = pageInfo.PageIndex;
            PageSize = pageInfo.PageSize;
            TotalItems = pageInfo.TotalItems;
            TotalPages = pageInfo.TotalPages;
        }
    }
}
