/*
    CreatedBy: DucDMD
    Date: 25/05/2024

    @PageIndex int
    @PageSize int
    @Status int
    @StartDate DATETIME
    @EndDate DATETIME
    @Available boolean
*/
-- SET @PageIndex = 1;
-- SET @PageSize = 12;
-- SET @Status = 1; -- 1 : active ; 2: locked ; 3: cancelled/deleted
-- SET @StartDate = NULL;
-- SET @EndDate = NULL;
-- SET @Available = TRUE; -- number_of_used < usage_limit or not

WITH FilteredPromotions AS (
    SELECT
        id,
        banner_url,
        amount_rate,
        minimum_order_value,
        maximum_apply_value,
        amount_value,
        apply_type,
        start_date,
        end_date,
        usage_limit,
        number_of_used,
        created_date,
        updated_date,
        status,
        ROW_NUMBER() OVER (
            ORDER BY
                CASE WHEN apply_type = 1 THEN 0 ELSE 1 END,
                CASE WHEN apply_type = 1 THEN amount_rate ELSE amount_value END DESC
        ) AS RowNum,
        COUNT(id) OVER () AS TotalItems
    FROM
        v_foody.platform_promotion
    WHERE
        CURTIME() BETWEEN start_date AND end_date
        AND usage_limit > number_of_used
        AND (@Status = 0 OR status = @Status)
        AND (@StartDate IS NULL OR start_date >= @StartDate)
        AND (@EndDate IS NULL OR start_date <= @EndDate)
        AND (@Available = FALSE OR number_of_used < usage_limit)
)

SELECT
    id AS Id,    
    start_date AS StartDate,
    end_date AS EndDate,
    banner_url AS BannerUrl,
    
    apply_type AS ApplyType,
    amount_rate AS AmountRate,
    amount_value AS AmountValue,
    minimum_order_value AS MinimumOrderValue,
    maximum_apply_value AS MaximumApplyValue,

    usage_limit AS UsageLimit,
    number_of_used AS NumberOfUsed,
    created_date AS CreatedDate,
    updated_date AS UpdatedDate,
    status AS Status,
    TotalItems,
    CEILING(TotalItems * 1.0 / @PageSize) AS TotalPages
FROM
    FilteredPromotions
WHERE
    RowNum BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize
ORDER BY
    RowNum ASC;
