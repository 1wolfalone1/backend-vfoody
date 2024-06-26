﻿/*
    CreatedBy: ThongNV
    Date: 05/06/2024

    @ShopIds int[]
*/

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
        b.name AS building_name,
        s.account_id,
        (s.total_star / s.total_rating) AS avg_rating,
        ROW_NUMBER() OVER (ORDER BY (s.total_star / s.total_rating) DESC) AS RowNum,
        COUNT(s.id) OVER () AS TotalItems
    FROM
        v_foody.shop s
    JOIN
        v_foody.building b ON s.building_id = b.id
    WHERE
        s.status = true
        AND s.id IN @ShopIds
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
    ROUND(avg_rating, 1) AS Rating
FROM
    ShopRatings
ORDER BY
    RowNum ASC;
