/*
 CreatedBy: ThongNV
 Date: 15/07/2024
 
 @ShopId 
 @ShopName
 @Status
 @DateFrom
 @DateTo
 @OrderBy
 
 */
-- SET	@ShopId = 1;
-- SET @ShopName = '';
-- SET @Status = 1;
-- SET @DateFrom = NULL;
-- SET @DateTo = NULL;
-- SET	@OrderBy = 0;
-- SET	@OrderMode = 0;
-- SET @PageIndex = 1;
-- SET	@PageSize = 10;
WITH ShopWithdrawal AS (
	SELECT 
		s.id AS shop_id,
		s.name AS shop_name,
		s.logo_url,
		s.banner_url,
		s.balance,
		s.phone_number,
		a.email,
		swr.id AS request_id,
		swr.requested_amount,
		swr.status,
		swr.bank_code,
		swr.bank_short_name,
		swr.bank_account_number,
		swr.note,
		swr.requested_date,
		swr.processed_date,
		ROW_NUMBER() OVER (
            ORDER BY
                swr.updated_date DESC
        ) AS row_num,
            COUNT(swr.id) OVER () AS total_items
	FROM 
	shop_withdrawal_requests swr 
	INNER JOIN shop s ON swr.shop_id = s.id
	INNER JOIN account a ON s.account_id = a.id
	WHERE 
		(@Status = 0
		OR swr.status = @Status)
		AND 
		(
			@DateFrom IS NULL 
			OR 
			@DateTo IS NULL 
			OR
            DATE_FORMAT(swr.created_date, '%Y-%m-%d') BETWEEN @DateFrom
            AND @DateTo
		)
		AND (
		@ShopId = 0
		OR s.id = @ShopId
		)
		AND  (
		@ShopName IS NULL 
		OR @ShopName = ''
		OR s.name LIKE CONCAT('%', @ShopName, '%')
		)
	ORDER BY
		IF(
		@OrderBy = 0
		AND @OrderMode = 0,
		swr.updated_date,
		NULL) DESC,
		IF(
		@OrderBy = 1
		AND @OrderMode = 0,
		swr.requested_amount,
		NULL) DESC,
		IF(
		@OrderBy = 2
		AND @OrderMode = 0,
		swr.bank_code,
		NULL) DESC,
		IF(
		@OrderBy = 0
		AND @OrderMode = 1,
		swr.updated_date,
		NULL) ASC,
		IF(
		@OrderBy = 1
		AND @OrderMode = 1,
		swr.requested_amount,
		NULL) ASC,
		IF(
		@OrderBy = 2
		AND @OrderMode = 1,
		swr.bank_code,
		NULL) ASC
)
SELECT
	shop_id AS ShopId,
	shop_name AS ShopName,
	logo_url AS LogoUrl,
	banner_url AS BannerUrl,
	balance AS Balance,
	phone_number AS PhoneNumber,
	email AS Email,
	total_items AS TotalItems,
	request_id AS RequestId,
	requested_amount AS RequestedAmount,
	status AS Status,
	bank_code AS BankCode,
	bank_short_name AS BankShortName,
	bank_account_number AS BankAccountNumber,
	note AS Note,
	requested_date AS RequestedDate,
	processed_date AS ProcessedDate
FROM 
ShopWithdrawal
WHERE
    row_num BETWEEN (@PageIndex - 1) * @PageSize + 1
    AND @PageIndex * @PageSize;
