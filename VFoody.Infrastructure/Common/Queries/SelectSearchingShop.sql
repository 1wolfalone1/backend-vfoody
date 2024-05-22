/*
    CreatedBy: DucDMD
    Date: 21/05/2024

    @PageIndex int
    @PageSize int
    @SearchText string
    @OrderType int
    @CurrentBuildingId int
*/
-- SET @PageIndex = 1;
-- SET @PageSize = 12;
-- SET @SearchText = '';
-- SET @OrderType = 1; -- 0: Random, 1: Rating, 2: TotalOrder, 3: Distance
-- SET @CurrentBuildingId = 1;

WITH TargetShopIdList AS (
    SELECT DISTINCT
        shop_id AS ShopId
    FROM
        product
    INNER JOIN
        shop ON product.shop_id = shop.id
    WHERE
        shop.status = true
        AND shop.active = true
        AND product.status = true
        AND (shop.name LIKE CONCAT('%', @SearchText, '%') OR product.name LIKE CONCAT('%', @SearchText, '%'))
),

TargetShop AS (
    SELECT
        s.id,
        s.name,
        s.logo_url,
        s.banner_url,
        s.description,
        s.balance,
        s.phone_number,
        s.active_from,
        s.active_to,
        s.active,
        s.total_order,
        s.total_product,
        s.total_rating,
        s.total_star,
        s.status,
        s.minimum_value_order_freeship,
        s.shipping_fee,
        s.building_id,
        b.name AS building_name,
        s.account_id,
        (s.total_star / s.total_rating) AS avg_rating,
        ROW_NUMBER() OVER (
            ORDER BY
                CASE WHEN s.id IN (SELECT ShopId FROM TargetShopIdList) THEN 0 ELSE 1 END ASC, -- Put shops from TargetShopIdList on top
                CASE WHEN @OrderType = 0 THEN RAND()
                     WHEN @OrderType = 1 THEN (s.total_star / s.total_rating)
                     WHEN @OrderType = 2 THEN s.total_order
                     WHEN @OrderType = 3 THEN (
                        SELECT
                            COALESCE(MIN(d.distance), 99999)
                        FROM
                            distance d
                        WHERE
                            d.building_id_from = @CurrentBuildingId
                            AND d.building_id_to = s.building_id
                    ) END DESC
        ) AS RowNum,
        COUNT(s.id) OVER () AS TotalItems
    FROM
        shop s
    LEFT JOIN
        building b ON s.building_id = b.id
)

SELECT
    id AS Id,
    name AS Name,
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
    building_id AS BuildingId,
    building_name AS BuildingName,
    account_id AS AccountId,
    avg_rating AS Rating,
    TotalItems,
    CEILING(TotalItems * 1.0 / @PageSize) AS TotalPages
FROM
    TargetShop
WHERE
    RowNum BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize
ORDER BY
    RowNum ASC;
