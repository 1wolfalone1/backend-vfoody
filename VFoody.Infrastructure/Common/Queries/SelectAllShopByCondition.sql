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
    ORDER BY
        CASE WHEN @Direction = 1 THEN -- Assuming 1 is for ASC and 2 for DESC
            CASE @OrderBy
                WHEN 1 THEN s.id
                WHEN 2 THEN s.name
                WHEN 3 THEN a.last_name
                WHEN 4 THEN s.phone_number
                WHEN 5 THEN s.active
                WHEN 6 THEN s.status
                WHEN 7 THEN s.total_order
                WHEN 8 THEN s.total_product
                WHEN 9 THEN s.created_date
            END
        END ASC,
        CASE WHEN @Direction = 2 THEN
            CASE @OrderBy
                WHEN 1 THEN s.id
                WHEN 2 THEN s.name
                WHEN 3 THEN a.last_name
                WHEN 4 THEN s.phone_number
                WHEN 5 THEN s.active
                WHEN 6 THEN s.status
                WHEN 7 THEN s.total_order
                WHEN 8 THEN s.total_product
                WHEN 9 THEN s.created_date
            END
        END DESC
        LIMIT @PageSize OFFSET @Offset
)
SELECT
    p.*,
    t.TotalCount
FROM
    PaginatedShopsCTE p,
    TotalCountCTE t;
