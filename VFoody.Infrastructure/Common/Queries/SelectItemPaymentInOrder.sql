/*
 CreatedBy: ThongNV
 Date: 12/06/2024
 SET @OrderId = 27;
 
 */
SELECT
    p.name AS Name,
    od.quantity AS Quantity,
    ((SUM(odo.price) + od.price) * od.quantity) AS Price
FROM
    `order` o
    INNER JOIN order_detail od ON o.id = od.order_id
    INNER JOIN product p ON od.product_id = p.id
    INNER JOIN order_detail_option odo ON od.id = odo.order_detail_id
WHERE
    o.id = @OrderId
GROUP BY
    p.name,
    od.quantity,
    od.price;