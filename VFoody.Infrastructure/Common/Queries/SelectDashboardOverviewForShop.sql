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
        SUM(
            CASE
                WHEN o.shop_promotion_id IS NOT NULL THEN o.total_price + o.shipping_fee - o.total_promotion -- Revenue after shop promotion discount
                ELSE o.total_price + o.shipping_fee -- Full price if platform or personal promotion is applied
            END
        )
    FROM
        `order` o
        INNER JOIN `transaction` t ON o.transaction_id = t.id
    WHERE
        o.shop_id = @ShopId
        AND o.status = 4
        AND o.is_refund = 0
        AND DATE_FORMAT(o.created_date, '%Y-%m-%d') BETWEEN DATE_FORMAT(@DateFrom, '%Y-%m-%d')
        AND DATE_FORMAT(@DateTo, '%Y-%m-%d')
),
TotalOrderSuccess AS (
    SELECT
        COUNT(id)
    FROM
        `order` o
    WHERE
        o.shop_id = @ShopId
        AND o.status = 4
      AND DATE_FORMAT(o.created_date, '%Y-%m-%d') BETWEEN DATE_FORMAT(@DateFrom, '%Y-%m-%d')
        AND DATE_FORMAT(@DateTo, '%Y-%m-%d')
),
TotalOrderCancel AS (
    SELECT
        COUNT(id)
    FROM
        `order` o
    WHERE
        o.shop_id = @ShopId
        AND o.status IN (5, 6, 7)
        AND DATE_FORMAT(o.created_date, '%Y-%m-%d') BETWEEN DATE_FORMAT(@DateFrom, '%Y-%m-%d')
        AND DATE_FORMAT(@DateTo, '%Y-%m-%d')
),
TotalCustomerOrder AS (
    SELECT
        DISTINCT account_id
    FROM
        `order` o
    WHERE
        o.shop_id = @ShopId
        AND o.status = 4
        AND DATE_FORMAT(o.created_date, '%Y-%m-%d') BETWEEN DATE_FORMAT(@DateFrom, '%Y-%m-%d')
        AND DATE_FORMAT(@DateTo, '%Y-%m-%d')
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