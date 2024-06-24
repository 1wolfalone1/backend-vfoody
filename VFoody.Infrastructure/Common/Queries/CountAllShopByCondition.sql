/*
    CreatedBy: TienPH
    Date: 25/06/2024

    @DeleteStatus int
    @OrderSuccessful int
    @SearchValue string
    @FilterByTime int
    @DateFrom datetime
    @DateTo datetime
*/

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
