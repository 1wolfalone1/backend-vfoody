/*
    CreatedBy: TienPH
    Date: 01/07/2024

    @DeleteStatus int
    @ShopId int
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
    id AS Id,
    title AS title,
    description AS Description,
    amount_rate AS AmountRate,
    minimum_order_value AS MinimumOrderValue,
    maximum_apply_value AS MaximumApplyValue,
    amount_value AS AmountValue,
    apply_type AS ApplyType,
    start_date AS StartDate,
    end_date AS EndDate,
    usage_limit AS UsageLimit,
    number_of_used AS NumberOfUsed,
    status AS Status,
    created_date AS CreatedDate,
    updated_date AS UpdatedDate
FROM
    shop_promotion sp
WHERE
    status != @DeleteStatus
    AND shop_id = @ShopId
    AND (@SearchValue IS NULL OR sp.title LIKE CONCAT('%', @SearchValue, '%'))
    AND (@FilterByTime IS NULL OR @FilterByTime = 0 OR sp.created_date >= NOW() - INTERVAL @FilterByTime DAY)
    AND (
        (@DateFrom IS NULL AND @DateTo IS NOT NULL AND sp.end_date <= @DateTo) OR
        (@DateFrom IS NOT NULL AND @DateTo IS NULL AND sp.start_date >= @DateFrom) OR
        (@DateFrom IS NOT NULL AND @DateTo IS NOT NULL AND sp.start_date >= @DateFrom AND sp.end_date <= @DateTo) OR
        (@DateFrom IS NULL AND @DateTo IS NULL)
    )
-- Assuming 1 is for ASC and 2 for DESC
ORDER BY
    IF(@OrderBy = 1 AND @Direction = 1, sp.id, NULL) ASC,
    IF(@OrderBy = 2 AND @Direction = 1, sp.title, NULL) ASC,
    IF(@OrderBy = 3 AND @Direction = 1, sp.description, NULL) ASC,
    IF(@OrderBy = 4 AND @Direction = 1, sp.amount_rate, NULL) ASC,
    IF(@OrderBy = 5 AND @Direction = 1, sp.minimum_order_value, NULL) ASC,
    IF(@OrderBy = 6 AND @Direction = 1, sp.maximum_apply_value, NULL) ASC,
    IF(@OrderBy = 7 AND @Direction = 1, sp.amount_value, NULL) ASC,
    IF(@OrderBy = 8 AND @Direction = 1, sp.apply_type, NULL) ASC,
    IF(@OrderBy = 9 AND @Direction = 1, sp.start_date, NULL) ASC,
    IF(@OrderBy = 10 AND @Direction = 1, sp.end_date, NULL) ASC,
    IF(@OrderBy = 11 AND @Direction = 1, sp.usage_limit, NULL) ASC,
    IF(@OrderBy = 12 AND @Direction = 1, sp.number_of_used, NULL) ASC,
    IF(@OrderBy = 13 AND @Direction = 1, sp.status, NULL) ASC,
    IF(@OrderBy = 14 AND @Direction = 1, sp.created_date, NULL) ASC,
    IF(@OrderBy = 15 AND @Direction = 1, sp.updated_date, NULL) ASC,
    IF(@OrderBy = 1 AND @Direction = 2, sp.id, NULL) DESC,
    IF(@OrderBy = 2 AND @Direction = 2, sp.title, NULL) DESC,
    IF(@OrderBy = 3 AND @Direction = 2, sp.description, NULL) DESC,
    IF(@OrderBy = 4 AND @Direction = 2, sp.amount_rate, NULL) DESC,
    IF(@OrderBy = 5 AND @Direction = 2, sp.minimum_order_value, NULL) DESC,
    IF(@OrderBy = 6 AND @Direction = 2, sp.maximum_apply_value, NULL) DESC,
    IF(@OrderBy = 7 AND @Direction = 2, sp.amount_value, NULL) DESC,
    IF(@OrderBy = 8 AND @Direction = 2, sp.apply_type, NULL) DESC,
    IF(@OrderBy = 9 AND @Direction = 2, sp.start_date, NULL) DESC,
    IF(@OrderBy = 10 AND @Direction = 2, sp.end_date, NULL) DESC,
    IF(@OrderBy = 11 AND @Direction = 2, sp.usage_limit, NULL) DESC,
    IF(@OrderBy = 12 AND @Direction = 2, sp.number_of_used, NULL) DESC,
    IF(@OrderBy = 13 AND @Direction = 2, sp.status, NULL) DESC,
    IF(@OrderBy = 14 AND @Direction = 2, sp.created_date, NULL) DESC,
    IF(@OrderBy = 15 AND @Direction = 2, sp.updated_date, NULL) DESC
LIMIT @PageSize OFFSET @Offset