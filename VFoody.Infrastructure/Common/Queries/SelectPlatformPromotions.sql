/*
    CreatedBy: DucDMD
    Date: 21/05/2024

    @PageIndex int
    @PageSize int
    @ShopId int
    @SearchText string
*/
-- SET @PageIndex = 1;
-- SET @PageSize = 12;
-- SET @ShopId = 1; -- ID of the shop to filter products
-- SET @SearchText = ''; -- Search keyword

WITH ProductSearch AS (
    SELECT
        p.id AS product_id,
        p.name AS product_name,
        p.description AS product_description,
        p.price AS product_price,
        p.image_url AS product_image_url,
        p.total_order AS product_total_order,
        p.status AS product_status,
        p.shop_id AS product_shop_id,
        s.name AS shop_name,
        s.logo_url AS shop_logo_url,
        s.active AS shop_active,
        s.active_from AS shop_active_from,
        s.active_to AS shop_active_to,
        ROW_NUMBER() OVER (
            ORDER BY 
                CASE 
                    WHEN p.name LIKE CONCAT('%', @SearchText, '%') THEN 0 
                    ELSE 1 
                END, -- Priority to products with SearchText in name
                p.total_order DESC
        ) AS RowNum,
        COUNT(p.id) OVER () AS TotalItems
    FROM
        v_foody.product p
    INNER JOIN
        v_foody.shop s ON p.shop_id = s.id
    WHERE
        p.status = true
        AND s.status = true
        AND p.shop_id = @ShopId
)

SELECT
    product_id AS Id,
    product_name AS Name,
    product_description AS Description,
    product_price AS Price,
    product_image_url AS ImageUrl,
    product_total_order AS TotalOrder,
    product_status AS Status,
    product_shop_id AS ShopId,
    shop_name AS ShopName,
    shop_logo_url AS ShopLogoUrl,
    shop_active AS ShopActive,
    shop_active_from AS ShopActiveFrom,
    shop_active_to AS ShopActiveTo,
    TotalItems,
    CEILING(TotalItems * 1.0 / @PageSize) AS TotalPages
FROM
    ProductSearch
WHERE
    RowNum BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize
ORDER BY
    RowNum ASC;
