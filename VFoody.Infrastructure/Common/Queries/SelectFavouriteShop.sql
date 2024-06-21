/*
    CreatedBy: TienPH
    Date: 22/06/2024

    @ActiveStatus int
    @ShopIds List<int>
    @Offset int
    @PageSize int
*/

WITH TotalCountCTE AS (
    SELECT
    COUNT(*) AS TotalItems
    FROM
        shop s
    WHERE
        s.status = @ActiveStatus AND s.id IN @ShopIds
    ),
PaginatedShopsCTE AS (
    SELECT
        s.id AS Id,
        s.name AS Name,
        s.logo_url AS LogoUrl,
        s.banner_url AS BannerUrl,
        s.description AS Description,
        s.balance AS Balance,
        s.phone_number AS PhoneNumber,
        s.active_from AS ActiveFrom,
        s.active_to AS ActiveTo,
        s.active AS Active,
        s.total_order AS TotalOrder,
        s.total_product AS TotalProduct,
        s.total_rating AS TotalRating,
        s.total_star AS TotalStar,
        s.status AS Status,
        s.minimum_value_order_freeship AS MinimumValueOrderFreeship,
        s.shipping_fee AS ShippingFee,
        s.building_id AS BuildingId,
        b.name AS BuildingName,
        s.account_id AS AccountId,
        ROUND(s.total_star / s.total_rating, 1) AS Rating
    FROM
        shop s JOIN building b ON s.building_id = b.id
    WHERE
    s.status = @ActiveStatus AND s.id IN @ShopIds
    LIMIT @PageSize OFFSET @Offset
    )
SELECT
    p.*,
    t.TotalItems
FROM
    PaginatedShopsCTE p,
    TotalCountCTE t;