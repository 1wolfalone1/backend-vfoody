/*
    CreatedBy: ThongNV
    Date: 25/06/2024

    @Status int
    @ShopId int
    @PageSize int
    @PageIndex int
 */
WITH ShopOrder AS (
    SELECT
        o.id,
        o.account_id,
        o.created_date,
        ROW_NUMBER() OVER (
            ORDER BY
                updated_date DESC
        ) AS row_num,
            COUNT(id) OVER () AS total_items
    FROM
        `order` o
    WHERE 
        o.status = @Status
        AND o.shop_id = @ShopId
)
SELECT
    o.id AS Id,
    a.id AS CustomerId,
    a.last_name AS CustomerName,
    o.created_date AS CreatedDate,
    total_items AS TotalItems
FROM
    ShopOrder o
    INNER JOIN account a ON o.account_id = a.id
WHERE
    row_num BETWEEN (@PageIndex - 1) * @PageSize + 1
    AND @PageIndex * @PageSize;