/*
    CreatedBy: TienPH
    Date: 01/07/2024

    @ActiveStatus int
    @InActiveStatus int
    @DeleteStatus int
    @ShopId int
    @IsAvailable bool
    @SearchValue string
    @FilterByTime int
    @DateFrom datetime
    @DateTo datetime
    @OrderBy int
    @Direction int
    @Offset int
    @PageSize int
*/

SELECT
    COUNT(*) AS TotalCount
FROM
    shop_promotion sp
WHERE
    status != @DeleteStatus
    AND shop_id = @ShopId
    AND (
        (@IsAvailable = true AND status = @ActiveStatus AND end_date >= NOW() AND number_of_used < usage_limit)
        OR
        (@IsAvailable = false AND (status = @InActiveStatus OR (status = @ActiveStatus AND (end_date < NOW() OR number_of_used >= usage_limit))))
    )
    AND (@SearchValue IS NULL OR sp.title LIKE CONCAT('%', @SearchValue, '%'))
    AND (@FilterByTime IS NULL OR @FilterByTime = 0 OR sp.created_date >= NOW() - INTERVAL @FilterByTime DAY)
    AND (
        (@DateFrom IS NULL AND @DateTo IS NOT NULL AND sp.end_date <= @DateTo) OR
        (@DateFrom IS NOT NULL AND @DateTo IS NULL AND sp.start_date >= @DateFrom) OR
        (@DateFrom IS NOT NULL AND @DateTo IS NOT NULL AND sp.start_date >= @DateFrom AND sp.end_date <= @DateTo) OR
        (@DateFrom IS NULL AND @DateTo IS NULL)
    )