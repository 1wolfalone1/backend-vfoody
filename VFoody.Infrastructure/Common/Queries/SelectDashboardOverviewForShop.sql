/*
 CreatedBy: ThongNV
 Date: 07/01/2024
 
 @ShopId int
 @DateFrom int
 @DateTo int
 */
-- SET @ShopId:=1;
-- SET @DateFrom:='2024-01-01';
-- SET @DateTo:='2024-07-01';
WITH TotalRevenueOfShop AS (
    SELECT
        SUM(amount) AS total_revenue
    FROM
        `order` o
        INNER JOIN `transaction` t ON o.transaction_id = t.id
    WHERE
        o.shop_id = @ShopId
        AND o.status = 4
        AND o.is_refund = 0
        AND DATE_FORMAT(o.created_date, '%Y-%m-%d') BETWEEN @DateFrom
        AND @DateTo
),
TotalOrderSuccess AS (
    SELECT
        COUNT(id)
    FROM
        `order` o
    WHERE
        o.shop_id = @ShopId
        AND o.status = 4
),
TotalOrderCancel AS (
    SELECT
        COUNT(id)
    FROM
        `order` o
    WHERE
        o.shop_id = @ShopId
        AND o.status != 4
),
TotalCustomerOrder AS (
    SELECT
        DISTINCT account_id
    FROM
        `order` o
    WHERE
        o.shop_id = @ShopId
        AND o.status = 4
)
SELECT
    (
        SELECT
            *
        FROM
            TotalRevenueOfShop
    ) AS TotalRevenue,
    (
        SELECT
            *
        FROM
            TotalOrderSuccess
    ) AS TotalSuccessOrder,
    (
        SELECT
            *
        FROM
            TotalOrderCancel
    ) AS TotalOrderCancel,
    (
        SELECT
            COUNT(*)
        FROM
            TotalCustomerOrder
    ) AS TotalCustomerOrder;