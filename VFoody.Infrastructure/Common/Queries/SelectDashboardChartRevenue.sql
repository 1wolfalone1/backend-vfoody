/*
 CreatedBy: ThongNV
 Date: 12/06/2024
 
 @DateOfYear datetime
 */
-- SET @DateOfYear:='2024-05-01';
WITH Revenue AS (
    SELECT
        t.amount,
        o.created_date
    FROM
        `transaction` t
        INNER JOIN `order` o ON t.id = o.transaction_id
    WHERE
        o.status = 4 -- Successful
        AND o.is_refund = 0
        AND YEAR(o.created_date) = YEAR(@DateOfYear)
),
PreviousRevenue AS (
    SELECT
        t.amount,
        o.created_date
    FROM
        `transaction` t
        INNER JOIN `order` o ON t.id = o.transaction_id
    WHERE
        o.status = 4 -- Successful
        AND o.is_refund = 0
        AND YEAR(o.created_date) = YEAR(DATE_SUB(@DateOfYear, INTERVAL 1 YEAR))
),
TwelveMonthRevenue AS (
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 1
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 1
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 2
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 2
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 3
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 3
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 4
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 4
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 5
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 5
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 6
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 6
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 7
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 7
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 8
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 8
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 9
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 9
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 10
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 10
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 11
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 11
        ) AS last_year
    UNION
    ALL
    SELECT
        (
            SELECT
                SUM(amount)
            FROM
                Revenue
            WHERE
                MONTH(created_date) = 12
        ) AS this_year,
        (
            SELECT
                SUM(amount)
            FROM
                PreviousRevenue
            WHERE
                MONTH(created_date) = 12
        ) AS last_year
)
SELECT
    YEAR(@DateOfYear) AS ThisYear,
    YEAR(DATE_SUB(@DateOfYear, INTERVAL 1 YEAR)) AS LastYear,
    (
        SELECT
            SUM(amount)
        FROM
            Revenue
    ) AS ThisYearStr,
    (
        SELECT
            SUM(amount)
        FROM
            PreviousRevenue
    ) AS LastYearStr,
    (
        SELECT
            JSON_ARRAYAGG(
                JSON_OBJECT('ThisYearStr', this_year, 'LastYearStr', last_year)
            )
        FROM
            TwelveMonthRevenue
    ) AS TwelveMonthRevenueStr;