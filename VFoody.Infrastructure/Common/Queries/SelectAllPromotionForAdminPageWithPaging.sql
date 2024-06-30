/*
 CreatedBy: ThongNV
 Date: 19/06/2024
 
 @DateFrom DateTime
 @DateTo DateTo
 @Status int
 @ApplyType
 @Title
 @Description
 @PageIndex
 @PageSize
 @PromotionType int
 
 */
-- SET @DateFrom:='2024-06-01';
-- SET @DateTo:='2024-08-01';
-- SET @Status:=0;
-- SET @ApplyType:=0;
-- SET @Title:='';
-- SET @Description:='';
-- SET @PageIndex:=1;
-- SET @PageSize:=5;
-- SET @PromotionType:=0;
WITH PersonalPromotion AS (
    SELECT
        id,
        title,
        '' AS banner_url,
        description,
        amount_rate,
        minimum_order_value,
        maximum_apply_value,
        amount_value,
        apply_type,
        start_date,
        end_date,
        usage_limit,
        number_of_used,
        status,
        2 AS promotion_type,
        updated_date,
        created_date
    FROM
        person_promotion pp
    WHERE
        (
            @DateFrom IS NULL
            OR DATE_FORMAT(pp.start_date, '%Y-%m-%d') <= @DateTo
            AND @DateFrom <= DATE_FORMAT(pp.end_date, '%Y-%m-%d')
        )
        AND (
            @Status = 0
            OR pp.status = @Status
        )
        AND (
            @ApplyType = 0
            OR pp.apply_type = @ApplyType
        )
        AND (
            @Title IS NULL
            OR pp.title LIKE CONCAT('%', CONCAT(@Title, '%'))
        )
        AND (
            @Description IS NULL
            OR pp.description LIKE CONCAT('%', CONCAT(@Description, '%'))
        )
),
ShopPromotion AS (
    SELECT
        id,
        title,
        '' AS banner_url,
        description,
        amount_rate,
        minimum_order_value,
        maximum_apply_value,
        amount_value,
        apply_type,
        start_date,
        end_date,
        usage_limit,
        number_of_used,
        status,
        3 AS promotion_type,
        updated_date,
        created_date
    FROM
        shop_promotion sp
    WHERE
        (
            @DateFrom IS NULL
            OR DATE_FORMAT(sp.start_date, '%Y-%m-%d') <= @DateTo
            AND @DateFrom <= DATE_FORMAT(sp.end_date, '%Y-%m-%d')
        )
        AND (
            @Status = 0
            OR sp.status = @Status
        )
        AND (
            @ApplyType = 0
            OR sp.apply_type = @ApplyType
        )
        AND (
            @Title IS NULL
            OR sp.title LIKE CONCAT('%', CONCAT(@Title, '%'))
        )
        AND (
            @Description IS NULL
            OR sp.description LIKE CONCAT('%', CONCAT(@Description, '%'))
        )
),
PlatformPromotion AS (
    SELECT
        id,
        title,
        banner_url,
        description,
        amount_rate,
        minimum_order_value,
        maximum_apply_value,
        amount_value,
        apply_type,
        start_date,
        end_date,
        usage_limit,
        number_of_used,
        status,
        1 AS promotion_type,
        updated_date,
        created_date
    FROM
        platform_promotion pp
    WHERE
        (
            @DateFrom IS NULL
            OR DATE_FORMAT(pp.start_date, '%Y-%m-%d') <= @DateTo
            AND @DateFrom <= DATE_FORMAT(pp.end_date, '%Y-%m-%d')
        )
        AND (
            @Status = 0
            OR pp.status = @Status
        )
        AND (
            @ApplyType = 0
            OR pp.apply_type = @ApplyType
        )
        AND (
            @Title IS NULL
            OR pp.title LIKE CONCAT('%', CONCAT(@Title, '%'))
        )
        AND (
            @Description IS NULL
            OR pp.description LIKE CONCAT('%', CONCAT(@Description, '%'))
        )
),
AllPromotion AS (
    SELECT
        id,
        title,
        banner_url,
        description,
        amount_rate,
        minimum_order_value,
        maximum_apply_value,
        amount_value,
        apply_type,
        start_date,
        end_date,
        usage_limit,
        number_of_used,
        status,
        promotion_type,
        updated_date,
        created_date
    FROM
        PersonalPromotion
    UNION
    ALL
    SELECT
        *
    FROM
        PlatformPromotion
    UNION
    ALL
    SELECT
        *
    FROM
        ShopPromotion
),
AllPromotionWithPaging AS (
    SELECT
        id,
        title,
        banner_url,
        description,
        amount_rate,
        minimum_order_value,
        maximum_apply_value,
        amount_value,
        apply_type,
        start_date,
        end_date,
        usage_limit,
        number_of_used,
        status,
        promotion_type,
        updated_date,
        created_date,
        ROW_NUMBER() OVER (
            ORDER BY
                updated_date,
                created_date,
                start_date DESC
        ) AS row_num,
        COUNT(id) OVER () AS total_item
    FROM
        AllPromotion
    WHERE
        @PromotionType = 0
        OR promotion_type = @PromotionType
    ORDER BY
        updated_date,
        created_date,
        start_date DESC
)
SELECT
    id AS Id,
    title AS Title,
    banner_url AS BannerUrl,
    description AS Description,
    amount_rate AS AmountRate,
    minimum_order_value AS MinimumOrderValue,
    maximum_apply_value AS MaximumApplyValue,
    amount_value AS AmountValue,
    apply_type AS ApplyType,
    start_date AS StartDate,
    end_date AS EndDate,
    usage_limit AS UsageLimit,
    number_of_used AS NumberOfUsed,
    status AS Status,
    promotion_type AS PromotionType,
    total_item AS TotalItems,
    updated_date AS UpdatedDate,
    created_date AS CreatedDate
FROM
    AllPromotionWithPaging
WHERE
    row_num BETWEEN (@PageIndex - 1) * @PageSize + 1
    AND @PageIndex * @PageSize;