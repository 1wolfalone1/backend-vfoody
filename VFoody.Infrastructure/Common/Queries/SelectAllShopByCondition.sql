/*
    CreatedBy: TienPH
    Date: 20/06/2024

    @DeleteStatus int
    @OrderSuccessful int
    @SearchValue string
    @FilterByTime int
    @DateFrom datetime
    @DateTo datetime
    @OrderBy int
    @Direction int
    @Offset int
    @PageSize int
*/


WITH TotalCountCTE AS (
    SELECT
        COUNT(*) AS TotalCount
    FROM
        shop s
            JOIN account a ON
            s.account_id = a.id
    WHERE
        s.status != @DeleteStatus
    AND (@SearchValue IS NULL OR s.name LIKE CONCAT('%', @SearchValue, '%'))
    AND (@FilterByTime IS NULL OR @FilterByTime = 0 OR s.created_date >= NOW() - INTERVAL @FilterByTime DAY)
    AND ((@DateFrom IS NULL AND @DateTo IS NULL) OR (s.created_date BETWEEN @DateFrom AND @DateTo))
),
PaginatedShopsCTE AS (
    SELECT
        s.id AS Id,
        s.name AS ShopName,
        a.last_name AS ShopOwnerName,
        s.logo_url AS LogoUrl,
        s.banner_url AS BannerUrl,
        s.phone_number AS PhoneNumber,
        s.active AS Active,
        s.status AS Status,
        s.total_order AS TotalOrder,
        s.total_product AS TotalProduct,
        s.created_date AS CreatedDate,
        COALESCE((
            SELECT
                SUM(
                    CASE
                        WHEN o.shop_promotion_id IS NOT NULL THEN o.total_price - o.total_promotion -- Revenue after shop promotion discount
                        ELSE o.total_price -- Full price if platform or personal promotion is applied
                    END
                )
            FROM `order` o
            WHERE o.shop_id = s.id AND o.status = @OrderSuccessful AND o.is_refund = FALSE
        ), 0) AS ShopRevenue
    FROM
        shop s JOIN account a ON s.account_id = a.id
    WHERE
        s.status != @DeleteStatus
        AND (@SearchValue IS NULL OR s.name LIKE CONCAT('%', @SearchValue, '%'))
        AND (@FilterByTime IS NULL OR @FilterByTime = 0 OR s.created_date >= NOW() - INTERVAL @FilterByTime DAY)
        AND ((@DateFrom IS NULL AND @DateTo IS NULL) OR (s.created_date BETWEEN @DateFrom AND @DateTo))
    -- Assuming 1 is for ASC and 2 for DESC
    ORDER BY
        IF(@OrderBy = 1 AND @Direction = 1, s.id, NULL) ASC,
        IF(@OrderBy = 2 AND @Direction = 1, s.name, NULL) ASC,
        IF(@OrderBy = 3 AND @Direction = 1, a.last_name, NULL) ASC,
        IF(@OrderBy = 4 AND @Direction = 1, s.phone_number, NULL) ASC,
        IF(@OrderBy = 5 AND @Direction = 1, s.active, NULL) ASC,
        IF(@OrderBy = 6 AND @Direction = 1, s.status, NULL) ASC,
        IF(@OrderBy = 7 AND @Direction = 1, s.total_order, NULL) ASC,
        IF(@OrderBy = 8 AND @Direction = 1, s.total_product, NULL) ASC,
        IF(@OrderBy = 9 AND @Direction = 1, s.created_date, NULL) ASC,
        IF(@OrderBy = 10 AND @Direction = 1, ShopRevenue, NULL) ASC,
        IF(@OrderBy = 1 AND @Direction = 2, s.id, NULL) DESC,
        IF(@OrderBy = 2 AND @Direction = 2, s.name, NULL) DESC,
        IF(@OrderBy = 3 AND @Direction = 2, a.last_name, NULL) DESC,
        IF(@OrderBy = 4 AND @Direction = 2, s.phone_number, NULL) DESC,
        IF(@OrderBy = 5 AND @Direction = 2, s.active, NULL) DESC,
        IF(@OrderBy = 6 AND @Direction = 2, s.status, NULL) DESC,
        IF(@OrderBy = 7 AND @Direction = 2, s.total_order, NULL) DESC,
        IF(@OrderBy = 8 AND @Direction = 2, s.total_product, NULL) DESC,
        IF(@OrderBy = 9 AND @Direction = 2, s.created_date, NULL) DESC,
        IF(@OrderBy = 10 AND @Direction = 2, ShopRevenue, NULL) DESC,
        IF(@OrderBy IS NULL AND @Direction IS NULL, s.created_date, NULL) DESC
    LIMIT @PageSize OFFSET @Offset
)
SELECT
    p.*,
    t.TotalCount
FROM
    PaginatedShopsCTE p,
    TotalCountCTE t;
