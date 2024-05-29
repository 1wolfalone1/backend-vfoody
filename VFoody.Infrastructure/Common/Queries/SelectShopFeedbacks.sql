/*
 CreatedAt: 29/05/2024
 CreateBy:ThongNV
 
 -- @ShopId int
 -- @PageIndex int
 -- @PageSize int
 */


WITH WithFeedBack AS (
    SELECT
        id,
        account_id,
        order_id,
        rating,
        comment,
        created_date,
        updated_date
    FROM
        feedback
    WHERE
            order_id IN (
            SELECT
                id
            FROM
                `order` o
            WHERE
                    o.id = @ShopId
        )
),
     WithFeedbackAndOrderProductName  AS (
         SELECT
             order_id,
             GROUP_CONCAT(
                     name
                         ORDER BY
                name SEPARATOR ', '
                 ) AS product_names
         FROM
             (
                 SELECT
                     od.order_id,
                     p.name
                 FROM
                     order_detail od
                         INNER JOIN product p ON od.product_id = p.id
                 WHERE
                         od.order_id IN (
                         SELECT
                             id
                         FROM
                             `order` o
                         WHERE
                                 o.id = @ShopId
                     )
             ) AS WithFeedbackAndOrderProductName
         GROUP BY
             order_id
     )

SELECT
    a.id AS AccountId,
    CONCAT(a.last_name,CONCAT(" ", a.first_name))  AS AccountName,
    feed.id AS FeedbackId,
    feed.rating AS Rating,
    feed.comment AS Comment,
    feedOrder.product_names AS ProductOrders,
    feed.created_date AS CreatedDate
FROM
    WithFeedBack AS feed
    INNER JOIN account AS a ON feed.account_id = a.id
    INNER JOIN WithFeedbackAndOrderProductName AS feedOrder ON feed.order_id = feedOrder.order_id;