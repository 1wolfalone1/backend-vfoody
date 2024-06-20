/*
 
 @ShopId int
 @CustomerID int
 @Distance int 
 @OrderValue int
 @CurrentDate
 @PageIndex
 @PageSize
 @Mode 1 - Take satisfy  promotion 2 - unsatisfy
 
 */
-- SET @ShopId:=1;
-- SET @CustomerId:=2;
-- SET @Distance:=2;
-- SET @OrderValue:=30000;
-- SET @CurrentDate:='2024-06-01';
-- SET @Mode:=2;
WITH PersonalPromotion AS (
	SELECT
		id,
		title,
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
		2 AS promotion_type
	FROM
		person_promotion pp
	WHERE
		pp.account_id = @CustomerId
		AND @CurrentDate BETWEEN pp.start_date
		AND end_date
		AND pp.number_of_used < pp.usage_limit
		AND pp.status = 1 -- ACTIVE
),
ShopPromotion AS (
	SELECT
		id,
		title,
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
		3 AS promotion_type
	FROM
		shop_promotion sp
	WHERE
		sp.shop_id = @ShopId
		AND @CurrentDate BETWEEN sp.start_date
		AND end_date
		AND sp.number_of_used < sp.usage_limit
		AND sp.status = 1 -- ACTIVE
),
PlatformPromotion AS (
	SELECT
		id,
		title,
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
		1 AS promotion_type
	FROM
		platform_promotion pp
	WHERE
		@CurrentDate BETWEEN pp.start_date
		AND end_date
		AND pp.number_of_used < pp.usage_limit
		AND pp.status = 1 -- ACTIVE
),
AllPromotion AS (
	SELECT
		id,
		title,
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
        promotion_type
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
		ROW_NUMBER() OVER (
			ORDER BY
				start_date DESC
		) AS row_num,
		COUNT(id) OVER () AS total_item
	FROM
		AllPromotion
	WHERE
		(
			@Mode = 1
			AND @OrderValue >= minimum_order_value
		)
		OR (
			@Mode = 2
			AND @OrderValue < minimum_order_value
		)
)
SELECT
	id AS Id,
	title AS Title,
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
	total_item AS TotalItems
FROM
	AllPromotionWithPaging
WHERE
	row_num BETWEEN (@PageIndex - 1) * @PageSize + 1
	AND @PageIndex * @PageSize;