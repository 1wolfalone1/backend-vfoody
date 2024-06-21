/*
 CreatedBy: ThongNV
 Date: 21/06/2024
 
 @Status int
 @AccountId int
 @PageIndex int
 @PageSize int
 
 */
-- SET @Status:=1;
-- SET @AccountId:=2;
-- SET @PageIndex:=1;
-- SET @PageSize:=10;
WITH OrderCustomer AS (
    SELECT
        id,
        status,
        shipping_fee,
        note,
        total_price,
        total_promotion,
        full_name,
        phone_number,
        is_refund,
        refund_status,
        charge_fee,
        transaction_id,
        building_id,
        created_date,
        updated_date,
        shop_id
    FROM
        `order` o
    WHERE
        o.account_id = @AccountId
        AND (
            @Status = 0
            OR @Status = o.status
        )
    ORDER BY
        o.updated_date DESC
),
OrderCustomerWithShopName AS (
    SELECT
        ord.id AS order_id,
        ord.status,
        ord.shipping_fee,
        ord.total_price,
        ord.total_promotion,
        ord.created_date,
        ord.updated_date,
        s.id AS shop_id,
        s.name,
        (
            SELECT
                SUM(quantity)
            FROM
                order_detail AS od
            WHERE
                od.order_id = ord.id
        ) AS total_product_order,
        ROW_NUMBER() OVER (
            ORDER BY
                ord.created_date DESC
        ) AS row_num,
        COUNT(ord.id) OVER () AS total_items
    FROM
        OrderCustomer AS ord
        INNER JOIN shop AS s ON ord.shop_id = s.id
)
SELECT
    order_id AS OrderId,
    status AS Status,
    (total_price + shipping_fee - total_promotion) AS TotalOrderValue,
    created_date AS OrderDate,
    shop_id AS ShopId,
    name AS ShopName,
    total_product_order AS ProductOrderQuantity,
    total_items AS TotalItems
FROM
    OrderCustomerWithShopName
WHERE
    row_num BETWEEN (@PageIndex - 1) * @PageSize + 1
    AND @PageIndex * @PageSize;