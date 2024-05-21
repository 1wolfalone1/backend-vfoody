using ArtHubBO.Payload;

namespace VFoody.Domain.Shared;

public class PageResult<T>
{
    public PageInfo PageInfo { get; set; } = new PageInfo();
    public List<T> PageData { get; set; } = new List<T>();

    public PageResult<T> PageNoData(PageInfo pageInfo)
    {
        PageResult<T> result = new PageResult<T>();
        result.PageInfo = new PageInfo(pageInfo);
        result.PageInfo.TotalPages = 0;
        result.PageInfo.TotalItems = 0;
        result.PageData = new List<T>();
        return result;
    }

    public PageResult<T> Build(PageInfo pageInfo, int totalItems, List<T> pageData)
    {
        PageResult<T> result = new PageResult<T>();
        result.PageInfo = new PageInfo(pageInfo.PageIndex, pageInfo.PageSize, totalItems);
        result.PageData = pageData;
        return result;
    }
}
