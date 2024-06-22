/*
    CreatedBy: TienPH
    Date: 22/06/2024

    @DeleteStatus int
    @OrderSuccessful int
    @ShopId int
*/

SELECT
    s.id AS Id,
    s.name AS ShopName,
    s.description AS Description,
    a.last_name AS ShopOwnerName,
    s.active_from AS ActiveFrom,
    s.active_to AS ActiveTo,
    a.email AS Email,
    b.name AS Address,
    s.logo_url AS LogoUrl,
    s.banner_url AS BannerUrl,
    s.phone_number AS PhoneNumber,
    s.active AS Active,
    s.status AS Status,
    s.total_order AS TotalOrder,
    s.total_product AS TotalProduct,
    s.total_rating AS TotalRating,
    ROUND((s.total_star / s.total_rating), 1) AS AvgRating,
    s.created_date AS CreatedDate,
    COALESCE((
                 SELECT
                     SUM(
                             CASE
                                 WHEN o.shop_promotion_id IS NOT NULL THEN o.total_price - o.total_promotion -- Revenue after shop promotion discount
                                 ELSE o.total_price -- Full price if platform or personal promotion is applied
                                 END
                     )
                 FROM `order` o
                 WHERE o.shop_id = s.id AND o.status = @OrderSuccessful AND o.is_refund = FALSE
             ), 0) AS ShopRevenue
FROM
    shop s JOIN account a ON s.account_id = a.id JOIN building b ON s.building_id = b.id
WHERE
    s.id = @ShopId AND s.status != @DeleteStatus