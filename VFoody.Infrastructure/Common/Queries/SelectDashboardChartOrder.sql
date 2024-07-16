/*
 CreatedBy: ThongNV
 CreatedDate: 12/06/2024
 UpdatedDate: 19/06/2024
 
 @DateFrom datetime
 @DateTo datetime
 */
-- SET @DateFrom:='2024-01-01';
-- SET @DateTo:='2024-07-01';
WITH OrderStatus AS (
    SELECT
        o.total_price,
        o.total_promotion,
        o.status,
        t.amount
    FROM
        `order` o
        INNER JOIN  `transaction` t ON o.transaction_id = t.id
    WHERE
        DATE_FORMAT(o.created_date, '%Y-%m-%d') BETWEEN DATE_FORMAT(@DateFrom, '%Y-%m-%d') 
        AND DATE_FORMAT(@DateTo, '%Y-%m-%d') 
),
OrderSummary AS (
    SELECT
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
        ) AS total_of_order,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                status = 1 -- pending
        ) AS pending,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 2 -- confirmed
        ) AS confirmed,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 3 -- delivering
        ) AS delivering,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 4 -- successful
        ) AS successful,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 5 -- cancelled
        ) AS cancelled,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                status = 6 -- fail
        ) AS fail,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 7 -- reject
        ) AS reject,
        (
            SELECT
                SUM(amount)
            FROM
                OrderStatus
            WHERE
                status = 5 -- delivered
        ) AS total_successfull_amount,
        (
            SELECT
                (
                    SUM(amount) * (
                        SELECT
                            cc.commission_rate
                        FROM
                            commission_config cc
                        ORDER BY
                            cc.updated_date DESC
                        LIMIT
                            1
                    ) / 100
                )
            FROM
                OrderStatus
            WHERE
                status = 5 -- delivered
        ) AS revenue
)
SELECT
    total_of_order AS TotalOfOrder,
    pending AS Pending,
    confirmed AS Confirmed,
    delivering AS Delivering,
    successful AS Successful,
    cancelled AS Cancelled,
    fail AS Fail,
    reject AS Reject,
    total_successfull_amount AS TotalTradingAmount,
    revenue AS Revenue,
    @DateTo AS Day
FROM
    OrderSummary;