﻿/*
 CreatedBy: ThongNV
 Date: 11/06/2024
 
 @DateFrom datetime
 @DateTo datetime
 */
-- SET @DateFrom:='2024-01-01';
-- SET @DateTo:='2024-07-01';
WITH TotalUser AS (
    SELECT
        COUNT(a.id) AS total_users
    FROM
        account a
    WHERE
        a.role_id IN (
            SELECT
                id
            FROM
                role
            WHERE
                name = "Customer"
                OR name = "Shop"
        )
        AND DATE_FORMAT(a.created_date, '%Y-%m-%d') BETWEEN DATE_FORMAT(@DateFrom, '%Y-%m-%d')
        AND DATE_FORMAT(@DateTo, '%Y-%m-%d')
),
TotalTrading AS (
    SELECT
        SUM(amount) AS total_trading
    FROM
        `transaction` t
        INNER JOIN `order` o ON t.id = o.transaction_id
    WHERE
        t.status = 1 -- PAID SUCCESS
        AND o.status = 4 -- Successful
        AND o.is_refund = 0 -- No refund
        AND DATE_FORMAT(o.created_date, '%Y-%m-%d') BETWEEN DATE_FORMAT(@DateFrom, '%Y-%m-%d')
        AND DATE_FORMAT(@DateTo, '%Y-%m-%d')
),
TotalRevenue AS (
    SELECT
        (
            total_trading * (
                SELECT
                    cc.commission_rate
                FROM
                    commission_config cc
                ORDER BY
                    cc.updated_date DESC
                LIMIT
                    1
            ) / 100
        ) AS total_revenue
    FROM
        TotalTrading
),
TotalOrder AS (
    SELECT
        COUNT(o.id) AS total_order
    FROM
        `order` o
    WHERE
        o.status = 4 -- Successful
        AND DATE_FORMAT(o.created_date, '%Y-%m-%d') BETWEEN DATE_FORMAT(@DateFrom, '%Y-%m-%d')
        AND DATE_FORMAT(@DateTo, '%Y-%m-%d')
)
SELECT
    t.total_trading AS TotalTrading,
    (
        SELECT
            total_revenue
        FROM
            TotalRevenue
    ) AS TotalRevenue,
    (
        SELECT
            total_order
        FROM
            TotalOrder
    ) AS TotalOrder,
    (
        SELECT
            total_users
        FROM
            TotalUser
    ) AS TotalUser
FROM
    TotalTrading AS t;