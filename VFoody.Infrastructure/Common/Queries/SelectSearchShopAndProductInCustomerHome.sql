/*
 CreatedBy: ThongNV
 Date: 27/06/2024
 UpdatedDate: 01/07/2024
 
 @SearchText
 @CategoryId
 @OrderType 1 -- Price,  2 rating
 @PageIndex 
 @PageSize
 @AccountId
 @OrderMode 0 ASC 1 DESC
 @ProductSizeInShop int
 */
-- SET @PageIndex = 1;
-- SET @PageSize = 25;
-- SET @SearchText = NULL;
-- SET @OrderType = 2; -- 0: Random, -- 1 Price,  2 rating
-- SET @CategoryId = 0;
-- SET @AccountId = 2;
-- SET @OrderMode = 0; -- 0 ASC 1 DESC
-- SET @ProductSizeInShop = 10;
WITH ShopAndProduct AS (
    SELECT
        s.id AS shop_id,
        p.id AS product_id
    FROM
        shop s
        INNER JOIN product p ON s.id = p.shop_id
        LEFT JOIN product_category pc ON p.id = pc.product_id
        LEFT JOIN question q ON p.id = q.product_id
        LEFT JOIN `option` op ON q.id = op.question_id
    WHERE
        TIME(NOW()) BETWEEN TIME(
            CONCAT(
                SUBSTRING(LPAD(s.active_from, 4, '0'), 1, 2),
                ':',
                SUBSTRING(LPAD(s.active_from, 4, '0'), 3, 2)
            )
        )
        AND TIME(
            CONCAT(
                SUBSTRING(LPAD(s.active_to, 4, '0'), 1, 2),
                ':',
                SUBSTRING(LPAD(s.active_to, 4, '0'), 3, 2)
            )
        )
        AND s.status = 1 -- Active
        AND s.active = true
        AND p.status = 1
        AND (
            @CategoryId = 0
            OR pc.category_id = @CategoryId
        )
        AND (
            @SearchText IS NULL
            OR @SearchText = ''
            OR s.name LIKE CONCAT('%', @SearchText, '%')
            OR s.description LIKE CONCAT('%', @SearchText, '%')
            OR p.name LIKE CONCAT('%', @SearchText, '%')
            OR p.description LIKE CONCAT('%', @SearchText, '%')
            OR q.description LIKE CONCAT('%', @SearchText, '%')
            OR op.description LIKE CONCAT('%', @SearchText, '%')
        )
    GROUP BY
        s.id,
        p.id
),
DistictProductAndShopWithRowNumber AS (
    SELECT
        s.id AS shop_id,
        s.name AS shop_name,
        s.logo_url,
        s.banner_url,
        s.description,
        s.balance,
        s.phone_number,
        s.active_from,
        s.active_to,
        s.active,
        s.total_order,
        s.total_rating,
        s.total_star,
        s.total_product,
        s.status,
        s.minimum_value_order_freeship,
        s.shipping_fee,
        s.building_id,
        s.account_id,
        p.id AS product_id,
        p.name AS product_name,
        p.description AS product_description,
        p.price AS product_price,
        p.image_url AS product_image_url,
        p.total_order AS product_total_order,
        p.status AS product_status,
        (total_star / total_rating) AS avg_rating,
        ROW_NUMBER() OVER (PARTITION BY s.id) AS row_num
    FROM
        ShopAndProduct sp
        INNER JOIN shop s ON sp.shop_id = s.id
        INNER JOIN product p ON sp.product_id = p.id
    ORDER BY
        CASE
            WHEN @OrderMode = 0
            AND @OrderType = 0 THEN p.total_order
            WHEN @OrderMode = 0
            AND @OrderType = 1 THEN p.price
            WHEN @OrderMode = 0
            AND @OrderType = 2 THEN (s.total_star / s.total_rating)
        END ASC,
        CASE
            WHEN @OrderMode = 1
            AND @OrderType = 0 THEN p.total_order
            WHEN @OrderMode = 1
            AND @OrderType = 1 THEN p.price
            WHEN @OrderMode = 1
            AND @OrderType = 2 THEN s.total_star
        END DESC
),
ShopAndProductFitInSize AS (
    SELECT
        shop_id,
        shop_name,
        logo_url,
        banner_url,
        description,
        balance,
        phone_number,
        active_from,
        active_to,
        active,
        total_order,
        total_rating,
        total_star,
        total_product,
        status,
        minimum_value_order_freeship,
        shipping_fee,
        building_id,
        account_id,
        product_id,
        product_name,
        product_description,
        product_price,
        product_image_url,
        product_total_order,
        product_status,
        avg_rating,
        shop_id AS idd,
        DENSE_RANK() OVER (
            ORDER BY
                CASE
                    WHEN @OrderMode = 0
                    AND @OrderType = 0 THEN total_order
                    WHEN @OrderMode = 0
                    AND @OrderType = 1 THEN product_price
                    WHEN @OrderMode = 0
                    AND @OrderType = 2 THEN avg_rating
                END ASC,
                CASE
                    WHEN @OrderMode = 1
                    AND @OrderType = 0 THEN total_order
                    WHEN @OrderMode = 1
                    AND @OrderType = 1 THEN product_price
                    WHEN @OrderMode = 1
                    AND @OrderType = 2 THEN avg_rating
                END DESC
        ) AS rank_num
    FROM
        DistictProductAndShopWithRowNumber
    WHERE
        row_num BETWEEN 0
        AND @ProductSizeInShop
)
SELECT
    shop_id AS Id,
    shop_name AS Name,
    logo_url AS LogoUrl,
    banner_url AS BannerUrl,
    description AS Description,
    balance AS Balance,
    phone_number AS PhoneNumber,
    active_from AS ActiveFrom,
    active_to AS ActiveTo,
    active AS Active,
    total_order AS TotalOrder,
    total_product AS TotalProduct,
    total_rating AS TotalRating,
    total_star AS TotalStar,
    status AS Status,
    minimum_value_order_freeship AS MinimumValueOrderFreeship,
    shipping_fee AS ShippingFee,
    sp.building_id AS BuildingId,
    b.name AS BuildingName,
    (
        EXISTS(
            SELECT
                id
            FROM
                favourite_shop fs
            WHERE
                fs.shop_id = sp.shop_id
        )
    ) AS IsFavouriteShop,
    account_id AS AccountId,
    product_id AS ProductId,
    product_name AS ProductName,
    product_description AS Description,
    product_price AS Price,
    product_image_url AS ImageUrl,
    product_total_order AS TotalOrder,
    product_status AS Status,
    shop_id AS ShopId,
    (
        SELECT
            MAX(rank_num)
        FROM
            ShopAndProductFitInSize
    ) AS TotalItems
FROM
    ShopAndProductFitInSize sp
    INNER JOIN building b ON sp.building_id = b.id
WHERE
    rank_num BETWEEN (@PageIndex - 1) * @PageSize + 1
    AND @PageIndex * @PageSize;