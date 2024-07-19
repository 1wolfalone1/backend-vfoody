/*
    CreatedBy: TienPH
    Date: 20/06/2024

    @ExcludedShopStatus int
    @ExcludedAccountStatus int
    @Offset int
    @PageSize int
*/

SELECT
    o.id AS Id,
    s.name AS ShopName,
    a.last_name AS CustomerName,
    o.status AS Status,
    o.created_date AS OrderDate,
    (o.total_price - o.total_promotion) AS Price
FROM
    `order` o
        JOIN
    shop s ON o.shop_id = s.id
        JOIN
    account a ON o.account_id = a.id
WHERE a.status != @ExcludedAccountStatus AND s.status != @ExcludedShopStatus
ORDER BY o.created_date DESC, (o.total_price + o.shipping_fee +  - o.total_promotion) DESC
LIMIT
    @PageSize
OFFSET
    @Offset;