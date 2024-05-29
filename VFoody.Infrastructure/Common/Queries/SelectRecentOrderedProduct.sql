/*
    CreatedBy: DucDMD
    Date: 21/05/2024

    @UserPhoneNumber string (param for tempory, apply by current user session later)
    @PageIndex INT
    @PageSize INT
*/

-- SET @Email = 'john@example.com';
-- SET @PageIndex = 1;
-- SET @PageSize = 12;

WITH RecentOrderedProducts AS (
    SELECT
        p.id,
        p.name,
        p.description,
        p.price,
        p.image_url,
        p.total_order,
        p.status,
        p.shop_id,
        p.created_date,
        p.updated_date,
        s.name AS shop_name,
        s.logo_url AS shop_logo_url,
        s.active AS shop_active,
        s.active_from AS shop_active_from,
        s.active_to AS shop_active_to,
        ROW_NUMBER() OVER (ORDER BY o.created_date DESC) AS RowNum,
        COUNT(p.id) OVER () AS TotalItems
    FROM
        v_foody.order_detail od
        INNER JOIN v_foody.order o ON od.order_id = o.id
        INNER JOIN v_foody.product p ON od.product_id = p.id
        INNER JOIN v_foody.account a ON o.account_id = a.id
        INNER JOIN v_foody.shop s ON p.shop_id = s.id
    WHERE
        a.email = @Email
        AND p.status = 1
        AND s.status = TRUE
)

SELECT
    id AS Id,
    name AS Name,
    description AS Description,
    price AS Price,
    image_url AS ImageUrl,
    total_order AS TotalOrder,
    status AS Status,
    shop_id AS ShopId,
    created_date AS CreatedDate,
    updated_date AS UpdatedDate,
    shop_name AS ShopName,
    shop_logo_url AS ShopLogoUrl,
    shop_active AS ShopActive,
    shop_active_from AS ShopActiveFrom,
    shop_active_to AS ShopActiveTo,
    TotalItems,
    CEILING(TotalItems * 1.0 / @PageSize) AS TotalPages
FROM
    RecentOrderedProducts
WHERE
    RowNum BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize
ORDER BY
    RowNum ASC;
