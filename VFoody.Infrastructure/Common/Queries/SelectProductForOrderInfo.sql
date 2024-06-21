-- @AccountId
-- SET @OrderId:=25;
WITH ProductOrder AS (
    SELECT
        od.id AS order_detail_id,
        od.order_id,
        od.product_id,
        od.quantity AS product_quantity,
        od.price AS product_price,
        p.name,
        p.image_url,
        p.shop_id,
        p.status AS product_status
    FROM
        order_detail od
            INNER JOIN product p ON od.product_id = p.id
    WHERE od.order_id = @OrderID
)
        ,
     OrderInfo AS (
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
             b.name,
             b.longitude,
             b.latitude
         FROM
             `order` o
                 INNER JOIN `transaction` t ON o.transaction_id = t.id
                 INNER JOIN building b ON o.building_id = b.id
         WHERE o.id = @OrderId
     )

        ,
     ProductWithOptionTopping AS (
         SELECT
             p.order_id,
             p.product_id,
             product_quantity AS product_quantity,
             p.product_price AS product_price,
             p.name,
             p.image_url,
             p.shop_id,
             p.product_status AS product_status,
             q.id AS question_id,
             q.question_type,
             q.description AS q_description,
             odo.option_id,
             op.description op_description,
             op.image_url AS option_image,
             odo.price AS option_price
         FROM
             ProductOrder as p
                 INNER JOIN order_detail_option odo ON p.order_detail_id = odo.order_detail_id
                 INNER JOIN `option` op ON odo.option_id = op.id
                 INNER JOIN question q ON op.question_id = q.id
     )
SELECT
    order_id AS OrderId,
    product_id AS ProductId,
    product_quantity AS ProductQuantity,
    product_price AS ProductPrice,
    name AS ProductName,
    image_url AS ImageUrl,
    shop_id AS ShopId,
    product_status AS ProductStatus,
    question_id AS QuestionId,
    question_type AS QuestionType,
    q_description AS QueDescription,
    option_id AS OptionId,
    op_description AS OpDescription,
    option_image AS OptionImageUrl,
    option_price AS OptionPrice
FROM ProductWithOptionTopping;
