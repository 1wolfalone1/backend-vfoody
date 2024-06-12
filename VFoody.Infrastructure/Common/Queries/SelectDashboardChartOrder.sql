/*
 CreatedBy: ThongNV
 Date: 12/06/2024
 
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
        DATE_FORMAT(o.created_date, '%Y-%m-%d') BETWEEN @DateFrom
        AND @DateTo
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
                status IN (1, 2, 3, 4) -- OrderPLaced OrderConfirmed Preparing OutForDelivery
        ) AS pending,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                status IN (6, 7) -- Cancel Refund
        ) AS cancel,
        (
            SELECT
                SUM(total_price - total_promotion)
            FROM
                OrderStatus
            WHERE
                status = 5 -- Deliveried
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
                status = 5 -- Deliveried
        ) AS revenue
)
SELECT
    total_of_order AS TotalOfOrder,
    pending AS Pending,
    cancel AS Cancel,
    total_successfull_amount AS TotalSuccessfullAmount,
    revenue AS Revenue,
    @DateTo AS Day
FROM
    OrderSummary;