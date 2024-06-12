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
                status = 1 -- OrderPLaced OrderConfirmed Preparing OutForDelivery
        ) AS order_placed,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 2 -- OrderPLaced OrderConfirmed Preparing OutForDelivery
        ) AS order_confirmed,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 3 -- OrderPLaced OrderConfirmed Preparing OutForDelivery
        ) AS preparing,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 4 -- OrderPLaced OrderConfirmed Preparing OutForDelivery
        ) AS out_for_delivery,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 5 -- OrderPLaced OrderConfirmed Preparing OutForDelivery
        ) AS delivered,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                status = 6 -- Cancel Refund
        ) AS cancel,
        (
            SELECT
                COUNT(status)
            FROM
                OrderStatus
            WHERE
                    status = 7 -- Cancel Refund
        ) AS refund,
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
    order_placed AS OrderPLaced,
    order_confirmed AS OrderConfirmed,
    preparing AS Preparing,
    out_for_delivery AS OutForDelivery,
    delivered AS Delivered,
    cancel AS Cancel,
    refund AS Refund,
    total_successfull_amount AS TotalTradingAmount,
    revenue AS Revenue,
    @DateTo AS Day
FROM
    OrderSummary;