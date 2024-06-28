/*
 CreatedBy: ThongNV
 Date: 21/06/2024
 
 @OrderId int
 
 */
-- SET 
WITH OrderInfo AS (
    SELECT
        o.id AS order_id,
        o.status AS order_status,
        o.shipping_fee,
        o.total_price,
        o.total_promotion,
        o.full_name,
        o.phone_number,
        o.distance,
        o.duration_shipping,
        o.building_id,
        o.shop_promotion_id,
        o.platform_promotion_id,
        o.personal_promotion_id,
        o.shop_id,
        o.note,
        b.name,
        b.longitude,
        b.latitude,
        o.created_date
    FROM
        `order` o
            INNER JOIN `transaction` t ON o.transaction_id = t.id
            INNER JOIN building b ON o.building_id = b.id
    WHERE o.id = @OrderId
),
     Promotion AS (
         SELECT
             COALESCE(sp.id, pp1.id, pp2.id) AS id,
             COALESCE(sp.title, pp1.title, pp2.title) AS title,
             COALESCE(sp.amount_rate, pp1.amount_rate, pp2.amount_rate) AS amount_rate,
             COALESCE(sp.minimum_order_value, pp1.minimum_order_value, pp2.minimum_order_value) AS minimum_order_value,
             COALESCE(sp.maximum_apply_value, pp1.maximum_apply_value, pp2.maximum_apply_value) AS maximum_apply_value,
             COALESCE(sp.amount_value, pp1.amount_value, pp2.amount_value) AS amount_value,
             COALESCE(sp.apply_type, pp1.apply_type, pp2.apply_type) AS apply_type,
             COALESCE(sp.start_date, pp1.start_date, pp2.start_date) AS start_date,
             COALESCE(sp.end_date, pp1.end_date, pp2.end_date) AS end_date
         FROM
             OrderInfo o
                 LEFT JOIN shop_promotion sp ON o.shop_promotion_id = sp.id
                 LEFT JOIN platform_promotion pp1 ON o.platform_promotion_id = pp1.id
                 LEFT JOIN person_promotion pp2 ON o.personal_promotion_id = pp2.id
     )
SELECT
    orf.order_id AS OrderId,
    orf.order_status AS OrderStatus,
    orf.shipping_fee AS ShippingFee,
    orf.total_price AS TotalPrice,
    orf.total_promotion AS TotalPromotion,
    orf.full_name AS FullName,
    orf.phone_number AS PhoneNumber,
    orf.distance AS Distance,
    orf.duration_shipping AS DurationShipping,
    orf.created_date AS OrderDate,
    orf.shop_id AS ShopId,
    orf.note AS Note,
    orf.building_id AS BuildingId,
    orf.name AS Address,
    orf.longitude AS Longitude,
    orf.latitude AS Latitude,
    pro.id AS PromotionId,
    pro.title AS Title,
    pro.amount_rate AS AmountRate,
    pro.amount_value AS AmountValue,
    pro.minimum_order_value AS MinimumOrderValue,
    pro.maximum_apply_value AS MaximumApplyValue,
    pro.apply_type AS ApplyType,
    pro.start_date AS StartDate,
    pro.end_date AS EndDate
FROM
    OrderInfo AS orf
        LEFT JOIN
    Promotion AS pro ON TRUE;