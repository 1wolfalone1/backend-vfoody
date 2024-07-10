/*
 FileName: SelectShopFeedbacks.sql
 CreatedAt: 29/05/2024
 CreateBy:ThongNV
 
 -- @ShopId int
 -- @PageIndex int
 -- @PageSize int
 */
-- SET @ShopId = 1;
-- SET @PageIndex = 1;
-- SET @PageSize = 10;
WITH WithFeedBack AS (
    SELECT
        id,
        account_id,
        order_id,
        rating,
        comment,
        images_url,
        created_date,
        updated_date,
        ROW_NUMBER() OVER (
            ORDER BY
                created_date DESC
        ) AS RowNum,
        COUNT(id) OVER () AS TotalRecords
    FROM
        feedback
    WHERE
        order_id IN (
            SELECT
                id
            FROM
                `order` o
            WHERE
                o.shop_id = @ShopId
        )
),
WithFeedbackAndOrderProductName AS (
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
                        o.shop_id = @ShopId
                )
        ) AS WithFeedbackAndOrderProductName
    GROUP BY
        order_id
)
SELECT
    @PageIndex AS PageIndex,
    @PageSize AS PageSize,
    feed.TotalRecords AS NumberOfItems,
    a.id AS AccountId,
    CONCAT(a.last_name, CONCAT(" ", a.first_name)) AS AccountName,
    a.avatar_url AS AccountAvartar,
    feed.id AS FeedbackId,
    feed.rating AS Rating,
    feed.comment AS Comment,
    feed.images_url AS ImagesUrl,
    feedOrder.product_names AS ProductOrders,
    feed.created_date AS CreatedDate
FROM
    WithFeedBack AS feed
    INNER JOIN account AS a ON feed.account_id = a.id
    INNER JOIN WithFeedbackAndOrderProductName AS feedOrder ON feed.order_id = feedOrder.order_id
WHERE
    feed.RowNum BETWEEN (@PageIndex - 1) * @PageSize + 1
    AND @PageIndex * @PageSize;