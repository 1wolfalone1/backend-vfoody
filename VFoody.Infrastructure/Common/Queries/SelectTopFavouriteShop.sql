/*
    CreatedBy: DucDMD
    Date: 19/05/2024

    @PageIndex int
    @PageSize int
*/
-- SET @PageIndex = 1;
-- SET @PageSize = 12;

WITH ShopRatings AS (
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
        s.account_id,
        (s.total_rating / s.total_star) AS avg_rating,
        ROW_NUMBER() OVER (ORDER BY (s.total_rating / s.total_star) DESC) AS RowNum,
        COUNT(s.id) OVER () AS TotalItems
    FROM
        v_foody.shop s
    WHERE status = true
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
    account_id AS AccountId,
    avg_rating AS Rating,
    TotalItems,
    CEILING(TotalItems * 1.0 / @PageSize) AS TotalPages
FROM
    ShopRatings
WHERE
    RowNum BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize
ORDER BY
    RowNum ASC;
