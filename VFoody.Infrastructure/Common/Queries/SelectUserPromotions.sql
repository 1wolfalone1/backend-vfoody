/*
    CreatedBy: DucDMD
    Date: 25/05/2024

    @AccountId int
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
-- SET @AccountId = 1;

WITH FilteredPersonPromotions AS (
    SELECT
        p.id,
        p.amount_rate,
        p.minimum_order_value,
        p.maximum_apply_value,
        p.amount_value,
        p.apply_type,
        p.start_date,
        p.end_date,
        p.usage_limit,
        p.number_of_used,
        p.created_date,
        p.updated_date,
        p.status,
        ROW_NUMBER() OVER (
            ORDER BY
                CASE WHEN p.apply_type = 1 THEN 0 ELSE 1 END,
                CASE WHEN p.apply_type = 1 THEN p.amount_rate ELSE p.amount_value END DESC
        ) AS RowNum,
        COUNT(p.id) OVER () AS TotalItems
    FROM
        person_promotion p
    WHERE
        CURTIME() BETWEEN p.start_date AND p.end_date
        AND p.usage_limit > p.number_of_used
        AND (@Status = 0 OR p.status = @Status)
        AND (@StartDate IS NULL OR p.start_date >= @StartDate)
        AND (@EndDate IS NULL OR p.start_date <= @EndDate)
        AND (@Available = FALSE OR p.number_of_used < p.usage_limit)
        AND (@AccountId = 0 OR @AccountId = p.account_id)
)

SELECT
    id AS Id,    
    start_date AS StartDate,
    end_date AS EndDate,
    
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
    FilteredPersonPromotions
WHERE
    RowNum BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize
ORDER BY
    RowNum ASC;
