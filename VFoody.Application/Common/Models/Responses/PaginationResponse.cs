namespace VFoody.Application.Common.Models.Responses;

public class PaginationResponse<TEntity, TResponse> where TEntity : class where TResponse : class
{
    public PaginationResponse(IList<TResponse> items, int pageIndex, int pageSize, int totalOfPages)
    {  
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalOfPages = totalOfPages;
        NumberOfItems = items.Count;
        Items = items;
    }

    public PaginationResponse(IList<TEntity> items, int pageIndex, int pageSize, int totalOfPages, Func<TEntity, TResponse> mapper)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalOfPages = totalOfPages;
        NumberOfItems = items.Count;
        Items = items.Select(mapper).ToList();
    }

    public PaginationResponse(IQueryable<TEntity> source, int pageIndex, int pageSize, int totalOfPages, Func<TEntity, TResponse> mapper)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalOfPages = totalOfPages;

        var items = source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        NumberOfItems = items.Count;
        Items = items.Select(mapper).ToList();
    }

    public int PageIndex { get; }

    public int TotalOfPages { get; }

    public int PageSize { get; set; }

    public int NumberOfItems { get; }

    public bool HasPrevious => PageIndex > 1;

    public bool HasNext => PageIndex < TotalOfPages;

    public IList<TResponse> Items { get; }
}

public class PaginationResponse<TEntity> : PaginationResponse<TEntity, TEntity> where TEntity : class
{
    public PaginationResponse(IList<TEntity> items, int pageIndex, int pageSize, int totalOfPages)
        : base(items, pageIndex, pageSize, totalOfPages, entity => entity)
    {
    }

    public PaginationResponse(IList<TEntity> items, int pageIndex, int pageSize, int totalOfPages, Func<TEntity, TEntity> mapper)
        : base(items, pageIndex, pageSize, totalOfPages, mapper)
    {
    }

    public PaginationResponse(IQueryable<TEntity> source, int pageIndex, int pageSize, int totalOfPages, Func<TEntity, TEntity> mapper)
        : base(source, pageIndex, pageSize, totalOfPages, mapper)
    {
    }

    public PaginationResponse(IQueryable<TEntity> source, int pageIndex, int pageSize, int totalOfPages)
        : base(source, pageIndex, pageSize, totalOfPages, entity => entity)
    {
    }
}
