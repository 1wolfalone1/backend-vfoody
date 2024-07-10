/*
 CreatedBy: ThongNV
 Date: 11/07/2024
 
 @ShopId int
 */
-- SET @ShopId = 1;
WITH FeedbackOfShop AS (
    SELECT
        f.id,
        f.rating
    FROM
        feedback f
        INNER JOIN `order` o ON f.order_id = o.id
    WHERE
        o.shop_id = @ShopId
),
TotalFeedbackExcellent AS (
    SELECT
        count(id)
    FROM
        FeedbackOfShop f
    WHERE
        f.rating = 5 -- excellent
),
TotalFeedbackGood AS (
    SELECT
        count(id)
    FROM
        FeedbackOfShop f
    WHERE
        f.rating = 4 -- good
),
TotalFeedbackAverage AS (
    SELECT
        count(id)
    FROM
        FeedbackOfShop f
    WHERE
        f.rating = 3 -- Average
),
TotalFeedbacBellowAverage AS (
    SELECT
        count(id)
    FROM
        FeedbackOfShop f
    WHERE
        f.rating = 2 -- bellow average
),
TotalFeedbackPoor AS (
    SELECT
        count(id)
    FROM
        FeedbackOfShop f
    WHERE
        f.rating = 1 -- poor
),
ShopAverage AS (
    SELECT
        ROUND(s.total_star / s.total_rating, 1)
    FROM
        shop s
    WHERE
        s.id = @ShopId
)
SELECT
    (
        SELECT
            count(id)
        FROM
            FeedbackOfShop
    ) AS ShopTotalFeedback,
    (
        SELECT
            *
        FROM
            ShopAverage
    ) AS ShopRatingAverage,
    (
        SELECT
            *
        FROM
            TotalFeedbackExcellent
    ) AS TotalExcellent,
    (
        SELECT
            *
        FROM
            TotalFeedbackGood
    ) AS TotalGood,
    (
        SELECT
            *
        FROM
            TotalFeedbackAverage
    ) AS TotalAverage,
    (
        SELECT
            *
        FROM
            TotalFeedbacBellowAverage
    ) AS TotalBellowAverage,
    (
        SELECT
            *
        FROM
            TotalFeedbackPoor
    ) AS TotalPoor;