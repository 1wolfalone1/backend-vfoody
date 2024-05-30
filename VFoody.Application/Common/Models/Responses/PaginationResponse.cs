namespace VFoody.Application.Common.Models.Responses;

public class PaginationResponse<TEntity, TResponse> where TEntity : class where TResponse : class
{
    public PaginationResponse()
    {
    }

    public PaginationResponse(IList<TResponse> items, int pageIndex, int pageSize, int numberOfItems)
    {  
        PageIndex = pageIndex;
        PageSize = pageSize;
        NumberOfItems = numberOfItems;
        Items = items;
    }

    public PaginationResponse(IList<TEntity> items, int pageIndex, int pageSize, int numberOfItems, Func<TEntity, TResponse> mapper)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        NumberOfItems = numberOfItems;
        Items = items.Select(mapper).ToList();
    }

    public PaginationResponse(IQueryable<TEntity> source, int pageIndex, int pageSize, int numberOfItems, Func<TEntity, TResponse> mapper)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
        NumberOfItems = numberOfItems;

        var items = source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        Items = items.Select(mapper).ToList();
    }

    public int PageIndex { get; set; }

    public int TotalOfPages
    {
        set{}
        get
        {
            return (int)Math.Ceiling((NumberOfItems * 1.0) / (PageSize * 1.0));
        }
    }

    public int PageSize { get; set; }

    public int NumberOfItems { get; set; }

    public bool HasPrevious => PageIndex > 1;

    public bool HasNext => PageIndex < TotalOfPages;

    public IList<TResponse> Items { get; set; }
}

public class PaginationResponse<TEntity> : PaginationResponse<TEntity, TEntity> where TEntity : class
{
    public PaginationResponse()
    {
    }

    public PaginationResponse(IList<TEntity> items, int pageIndex, int pageSize, int numberOfItems)
        : base(items, pageIndex, pageSize, numberOfItems, entity => entity)
    {
    }

    public PaginationResponse(IList<TEntity> items, int pageIndex, int pageSize, int numberOfItems, Func<TEntity, TEntity> mapper)
        : base(items, pageIndex, pageSize, numberOfItems, mapper)
    {
    }

    public PaginationResponse(IQueryable<TEntity> source, int pageIndex, int pageSize, int numberOfItems, Func<TEntity, TEntity> mapper)
        : base(source, pageIndex, pageSize, numberOfItems, mapper)
    {
    }

    public PaginationResponse(IQueryable<TEntity> source, int pageIndex, int pageSize, int numberOfItems)
        : base(source, pageIndex, pageSize, numberOfItems, entity => entity)
    {
    }
}
