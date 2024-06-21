/*
    CreatedBy: TienPH
    Date: 20/06/2024

    @ExcludedShopStatus int
    @ExcludedAccountStatus int
*/

SELECT
    COUNT(1) AS TotalOrder
FROM
    `order` o
        JOIN
    shop s ON o.shop_id = s.id
        JOIN
    account a ON o.account_id = a.id
WHERE a.status != @ExcludedAccountStatus AND s.status != @ExcludedShopStatus;