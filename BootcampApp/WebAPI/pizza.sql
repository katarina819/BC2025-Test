CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE "Users" (
   "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
   "Name" VARCHAR(100) NOT NULL,
   "Email" VARCHAR(100) NOT NULL,
	"Age" INT NOT NULL
);

ALTER TABLE "Users"
ADD COLUMN "Age" INT;
UPDATE "Users"
SET "Age" = CASE "Name"
   WHEN 'Ana' THEN 25
   WHEN 'Ivan' THEN 30
   WHEN 'Marko' THEN 28
   WHEN 'Petra' THEN 22
   WHEN 'Luka' THEN 35
   WHEN 'Maja' THEN 27
   WHEN 'Nikola' THEN 33
   WHEN 'Tina' THEN 24
   WHEN 'Filip' THEN 29
   WHEN 'Ivana' THEN 26
   ELSE NULL
END;


INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Ana', 'ana@example.com', 25) 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Ivan', 'ivan@example.com', 30) 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Marko', 'marko@example.com', 28) 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Petra', 'petra@example.com', 22) 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Luka', 'luka@example.com', 35) 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Maja', 'maja@example.com', 27) 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Nikola', 'nikola@example.com', 33) 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Tina', 'tina@example.com', 24) 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Filip', 'filip@example.com', 29) 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES (uuid_generate_v4(), 'Ivana', 'ivana@example.com', 26) 
RETURNING "Id";

select * from "Users";


INSERT INTO "Users" ("Id", "Name", "Email", "Age") 
VALUES ('011c1f93-abd2-43e2-9634-cb24decff7e4', 'Nikola', 'nikola@example.com', 33);

INSERT INTO "Users"("Id", "Name", "Email", "Age") 
VALUES ('7beb4278-9cba-42c6-b1d8-765f388810ad', 'Tina', 'tina@example.com', 24);


INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('1fb4eecd-2ca7-472e-96bf-9d228a49836d', 'Ana', 'ana@example.com', 25);
INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('daa54377-0a81-4d9f-aa5f-e2fe3d2cf24b', 'Ivan', 'ivan@example.com', 30);
INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('e54cee70-3e5e-4e7c-a95c-780b58cd5926', 'Marko', 'marko@example.com', 28);
INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('69ae3bb2-781c-4e0c-b1a7-b662d65e80b2', 'Petra', 'petra@example.com', 22);
INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('e5e7b3bc-1986-47e3-a28d-ec8b739223b6', 'Luka', 'luka@example.com', 35);
INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('a4836e85-c454-4ec0-b1f4-4fe31cc5eea2', 'Maja', 'maja@example.com', 27);
INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('011c1f93-abd2-43e2-9634-cb24decff7e4', 'Nikola', 'nikola@example.com', 33);
INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('7beb4278-9cba-42c6-b1d8-765f388810ad', 'Tina', 'tina@example.com', 24);
INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('3f5bc139-40f5-4716-8feb-34faefa05bd5', 'Filip', 'filip@example.com', 29);
INSERT INTO "Users" ("Id", "Name", "Email", "Age") VALUES ('c51bcdbf-076a-48ac-b70a-91f60fe48945', 'Ivana', 'ivana@example.com', 26);

CREATE TABLE "UserProfiles" (
   "UserId" UUID PRIMARY KEY,
   "PhoneNumber" VARCHAR(20),
   "Address" TEXT,
   FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

INSERT INTO "UserProfiles"("UserId", "PhoneNumber", "Address") 
VALUES ('733ac241-17f1-450d-91a5-e7827c53729d', '+385995556677', 'Trg slobode 13, Osijek');
INSERT INTO "UserProfiles"("UserId", "PhoneNumber", "Address") 
VALUES ('d0456ebf-31c2-4b87-aa1d-087b42e8a0f6', '+385987878787', 'Sjenjak 2, Osijek');


INSERT INTO "UserProfiles" ("UserId", "PhoneNumber", "Address") VALUES
('1fb4eecd-2ca7-472e-96bf-9d228a49836d', '+385981112233', 'Vukovarska 12, Osijek'),         -- Ana
('daa54377-0a81-4d9f-aa5f-e2fe3d2cf24b', '+385981234567', 'Europske avenije 34, Osijek'),   -- Ivan
('e54cee70-3e5e-4e7c-a95c-780b58cd5926', '+385997776655', 'J. J. Strossmayera 5, Osijek'),   -- Marko
('69ae3bb2-781c-4e0c-b1a7-b662d65e80b2', '+385912223344', 'Kapucinska 88, Osijek'),         -- Petra
('e5e7b3bc-1986-47e3-a28d-ec8b739223b6', '+385923334455', 'Radićeva 9, Osijek'),            -- Luka
('a4836e85-c454-4ec0-b1f4-4fe31cc5eea2', '+385994445566', 'Županijska 77, Osijek'),         -- Maja
('011c1f93-abd2-43e2-9634-cb24decff7e4', '+385995556677', 'Trg slobode 13, Osijek'),        -- Nikola
('7beb4278-9cba-42c6-b1d8-765f388810ad', '+385987878787', 'Sjenjak 2, Osijek'),             -- Tina
('3f5bc139-40f5-4716-8feb-34faefa05bd5', '+385996969696', 'Retfalačka 4, Osijek'),          -- Filip
('c51bcdbf-076a-48ac-b70a-91f60fe48945', '+385991112233', 'Gornjodravska obala 5, Osijek'); -- Ivana

select * from "UserProfiles"; 
select * from notifications;
CREATE TABLE "PizzaItems" (
   "PizzaId" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
   "Name" VARCHAR(100) NOT NULL,
   "Size" VARCHAR(20),  -- npr. "Mala", "Srednja", "Velika"
   "Price" NUMERIC(10, 2) NOT NULL
);

INSERT INTO "PizzaItems" ("Name", "Size", "Price") VALUES
-- Margherita
('Margherita', 'Mala', 0),
('Margherita', 'Srednja', 0),
('Margherita', 'Velika', 0),

-- Capricciosa
('Capricciosa', 'Mala', 0),
('Capricciosa', 'Srednja', 0),
('Capricciosa', 'Velika', 0),

-- Pepperoni
('Pepperoni', 'Mala', 0),
('Pepperoni', 'Srednja', 0),
('Pepperoni', 'Velika', 0),

-- Vegetariana
('Vegetariana', 'Mala', 0),
('Vegetariana', 'Srednja', 0),
('Vegetariana', 'Velika', 0),

-- Quattro Formaggi
('Quattro Formaggi', 'Mala', 0),
('Quattro Formaggi', 'Srednja', 0),
('Quattro Formaggi', 'Velika', 0),

-- Funghi
('Funghi', 'Mala', 0),
('Funghi', 'Srednja', 0),
('Funghi', 'Velika', 0),

-- Hawaiian
('Hawaiian', 'Mala', 0),
('Hawaiian', 'Srednja', 0),
('Hawaiian', 'Velika', 0),

-- Diavola
('Diavola', 'Mala', 0),
('Diavola', 'Srednja', 0),
('Diavola', 'Velika', 0),

-- BBQ Chicken
('BBQ Chicken', 'Mala', 0),
('BBQ Chicken', 'Srednja', 0),
('BBQ Chicken', 'Velika', 0),

-- Tuna
('Tuna', 'Mala', 0),
('Tuna', 'Srednja', 0),
('Tuna', 'Velika', 0);


UPDATE "PizzaItems"
SET "Price" = CASE 
   -- Margherita
   WHEN "Name" = 'Margherita' AND "Size" = 'Mala' THEN 5.40
   WHEN "Name" = 'Margherita' AND "Size" = 'Srednja' THEN 6.50
   WHEN "Name" = 'Margherita' AND "Size" = 'Velika' THEN 7.80

   -- Capricciosa
   WHEN "Name" = 'Capricciosa' AND "Size" = 'Mala' THEN 7.10
   WHEN "Name" = 'Capricciosa' AND "Size" = 'Srednja' THEN 8.50
   WHEN "Name" = 'Capricciosa' AND "Size" = 'Velika' THEN 9.90

   -- Pepperoni
   WHEN "Name" = 'Pepperoni' AND "Size" = 'Mala' THEN 6.50
   WHEN "Name" = 'Pepperoni' AND "Size" = 'Srednja' THEN 7.80
   WHEN "Name" = 'Pepperoni' AND "Size" = 'Velika' THEN 9.10

   -- Vegetariana
   WHEN "Name" = 'Vegetariana' AND "Size" = 'Mala' THEN 5.90
   WHEN "Name" = 'Vegetariana' AND "Size" = 'Srednja' THEN 7.10
   WHEN "Name" = 'Vegetariana' AND "Size" = 'Velika' THEN 8.30

   -- Quattro Formaggi
   WHEN "Name" = 'Quattro Formaggi' AND "Size" = 'Mala' THEN 7.60
   WHEN "Name" = 'Quattro Formaggi' AND "Size" = 'Srednja' THEN 8.80
   WHEN "Name" = 'Quattro Formaggi' AND "Size" = 'Velika' THEN 9.20

   -- Funghi
   WHEN "Name" = 'Funghi' AND "Size" = 'Mala' THEN 5.80
   WHEN "Name" = 'Funghi' AND "Size" = 'Srednja' THEN 7.00
   WHEN "Name" = 'Funghi' AND "Size" = 'Velika' THEN 8.40

   -- Hawaiian
   WHEN "Name" = 'Hawaiian' AND "Size" = 'Mala' THEN 7.40
   WHEN "Name" = 'Hawaiian' AND "Size" = 'Srednja' THEN 8.90
   WHEN "Name" = 'Hawaiian' AND "Size" = 'Velika' THEN 10.50

   -- Diavola
   WHEN "Name" = 'Diavola' AND "Size" = 'Mala' THEN 6.90
   WHEN "Name" = 'Diavola' AND "Size" = 'Srednja' THEN 8.20
   WHEN "Name" = 'Diavola' AND "Size" = 'Velika' THEN 9.60

   -- BBQ Chicken
   WHEN "Name" = 'BBQ Chicken' AND "Size" = 'Mala' THEN 7.90
   WHEN "Name" = 'BBQ Chicken' AND "Size" = 'Srednja' THEN 9.50
   WHEN "Name" = 'BBQ Chicken' AND "Size" = 'Velika' THEN 11.10

   -- Tuna
   WHEN "Name" = 'Tuna' AND "Size" = 'Mala' THEN 6.70
   WHEN "Name" = 'Tuna' AND "Size" = 'Srednja' THEN 8.00
   WHEN "Name" = 'Tuna' AND "Size" = 'Velika' THEN 9.30

   ELSE "Price"
END;

ALTER TABLE "PizzaItems"
ADD COLUMN "IsVegetarian" BOOLEAN;


UPDATE "PizzaItems"
SET "IsVegetarian" = TRUE
WHERE "Name" IN ('Margherita', 'Vegetariana', 'Quattro Formaggi');

UPDATE "PizzaItems"
SET "IsVegetarian" = FALSE
WHERE "Name" IN ('Capricciosa', 'Pepperoni');

UPDATE "PizzaItems"
SET "IsVegetarian" = TRUE
WHERE "Name" IN ('Funghi');

UPDATE "PizzaItems"
SET "IsVegetarian" = FALSE
WHERE "Name" IN ('Hawaiian', 'Diavola', 'BBQ Chicken', 'Tuna');


SELECT * FROM "PizzaItems";

CREATE TABLE "Drinks" (
   "DrinkId" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
   "Name" VARCHAR(100) NOT NULL,
   "Size" VARCHAR(20),  -- npr. "0.5L", "1L", "0.33L"
   "Price" NUMERIC(10, 2) NOT NULL
);

ALTER TABLE "Drinks"
ADD COLUMN "IsSugarFree" BOOLEAN DEFAULT FALSE;

UPDATE "Drinks"
SET "IsSugarFree" = TRUE
WHERE "Name" IN ('Jamnica', 'Jana voda', 'Pepsi Max', 'Sensation Limeta-Kiwi');

UPDATE "Drinks"
SET "IsSugarFree" = FALSE
WHERE "Name" IN ('Coca-Cola', 'Fanta', 'Sprite', 'Cedevita Naranča', 'Ice Tea Breskva', 'Red Bull');

INSERT INTO "Drinks" ("Name", "Size", "Price") VALUES
('Coca-Cola', '0.5L', 2.50),
('Fanta', '0.5L', 2.50),
('Jamnica', '0.5L', 2.00),
('Sprite', '0.33L', 2.20),
('Jana voda', '0.5L', 2.10),
('Pepsi Max', '0.5L', 2.50),
('Cedevita Naranča', '0.33L', 2.30),
('Ice Tea Breskva', '0.5L', 2.40),
('Red Bull', '0.25L', 3.00),
('Sensation Limeta-Kiwi', '0.5L', 2.20);

SELECT * FROM "Drinks";
SELECT * FROM "DrinkOrderItems";

ALTER TABLE "DrinkOrderItems"
ADD COLUMN "CardPaymentTransactionId" VARCHAR(255);

ALTER TABLE drinks_orders
ADD COLUMN "CardPaymentTransactionId" VARCHAR(255);


CREATE TABLE "DrinkOrderItems" (
   "OrderItemId" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
   "OrderId" UUID NOT NULL,
   "DrinkId" UUID NOT NULL,
   "Quantity" INT NOT NULL,
   "UnitPrice" NUMERIC(10,2) NOT NULL,
   "TotalCost" NUMERIC(10,2) GENERATED ALWAYS AS ("Quantity" * "UnitPrice") STORED,
   FOREIGN KEY ("OrderId") REFERENCES "DrinksOrders"("OrderId") ON DELETE CASCADE,
   FOREIGN KEY ("DrinkId") REFERENCES "Drinks"("DrinkId") ON DELETE CASCADE
);


INSERT INTO "DrinkOrderItems" ("OrderId", "DrinkId", "Quantity", "UnitPrice")
VALUES ('e854be55-f99f-4fa0-be7f-87a78b1ce4d3', '81ee55ce-096b-49f1-b33f-f9370d2a396d', 1, 2.00);


INSERT INTO "DrinkOrderItems" ("OrderId", "DrinkId", "Quantity", "UnitPrice")
VALUES ('4adf9527-a452-4835-b036-d394ef8dafe7', '9a7d9d99-4565-4dea-8c39-b5ccb189ad65', 1, 2.10);

SELECT * FROM pizza_orders;


CREATE TABLE "pizza_orders" (
   "OrderId" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
   "UserId" UUID NOT NULL,
   "OrderDate" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
   FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

SELECT * FROM public.drinks_orders;

CREATE TABLE "DrinksOrders" (
   "OrderId" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
   "UserId" UUID NOT NULL,
   "OrderDate" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
   FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
	);

SELECT "OrderId" FROM public.drinks_orders;

SELECT * FROM notifications LIMIT 1;

ALTER TABLE "DrinksOrders" RENAME TO drinks_orders;

INSERT INTO "DrinksOrders" ("UserId")
VALUES ('1fb4eecd-2ca7-472e-96bf-9d228a49836d')
RETURNING "OrderId";

INSERT INTO "pizza_orders" ("UserId") VALUES
('1fb4eecd-2ca7-472e-96bf-9d228a49836d'),  -- Ana
('daa54377-0a81-4d9f-aa5f-e2fe3d2cf24b'),  -- Ivan
('e54cee70-3e5e-4e7c-a95c-780b58cd5926');  -- Marko

ALTER TABLE drinks_orders
ADD COLUMN "CardPaymentTransactionId" VARCHAR(100);


CREATE TABLE "PizzaOrderItems" (
   "OrderItemId" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
   "OrderId" UUID NOT NULL,
   "PizzaId" UUID NOT NULL,
   "Quantity" INT NOT NULL,
   "UnitPrice" NUMERIC(10, 2) NOT NULL,
   FOREIGN KEY ("OrderId") REFERENCES "pizza_orders"("OrderId") ON DELETE CASCADE,
   FOREIGN KEY ("PizzaId") REFERENCES "PizzaItems"("PizzaId") ON DELETE CASCADE
);


INSERT INTO "PizzaOrderItems" ("OrderId", "PizzaId", "Quantity", "UnitPrice") VALUES
('f3703dfb-0f11-4c96-862a-7863ff262de6', 'd787b292-6a7b-4177-8447-ecb2bd791267', 2, 7.90),
('c0a223f2-ff70-4241-ab23-1220398a9ca4', 'cda8e29d-10d3-4d18-a78d-d48f67e6727d', 1, 6.70),
('721d6f31-1863-4282-b1e9-5cef7b61c775', '1a358b81-4f2c-4c0f-9b88-c7de2620dbf3', 3, 5.40);

INSERT INTO "pizza_orders" ("UserId")
VALUES ('670dffdc-a4dd-47d2-8aac-be8bb00452e4')  
RETURNING "OrderId";

INSERT INTO "pizza_orders" ("UserId")
VALUES ('81ee6d58-4a84-4f0b-8539-565e4b55ad07')  
RETURNING "OrderId";

INSERT INTO "pizza_orders" ("UserId")
VALUES ('72926c3a-855f-45d9-812a-e8c8faafeecf')  
RETURNING "OrderId";


ALTER TABLE "PizzaOrderItems"
ADD COLUMN "TotalPrice" NUMERIC(10, 2) GENERATED ALWAYS AS ("Quantity" * "UnitPrice") STORED;

select * from "PizzaOrderItems";

/* SELECT "OrderId", "UserId", "OrderDate" FROM "pizza_orders";
*/
SELECT "PizzaId", "Name", "Size" FROM "PizzaItems";

--inner join
/* SELECT 
   po."OrderId",
   u."Name" AS "UserName",
   pi."Name" AS "PizzaName",
   poi."Quantity",
   poi."UnitPrice"
FROM "pizza_orders" po
INNER JOIN "Users" u ON po."UserId" = u."Id"
INNER JOIN "PizzaOrderItems" poi ON po."OrderId" = poi."OrderId"
INNER JOIN "PizzaItems" pi ON poi."PizzaId" = pi."PizzaId";
*/

--left join
/* SELECT 
   po."OrderId",
   u."Name" AS "UserName",
   pi."Name" AS "PizzaName",
   poi."Quantity",
   poi."UnitPrice"
FROM "pizza_orders" po
LEFT JOIN "Users" u ON po."UserId" = u."Id"
LEFT JOIN "PizzaOrderItems" poi ON po."OrderId" = poi."OrderId"
LEFT JOIN "PizzaItems" pi ON poi."PizzaId" = pi."PizzaId";
*/
--right join
SELECT 
   po."OrderId",
   u."Name" AS "UserName",
   pi."Name" AS "PizzaName",
   poi."Quantity",
   poi."UnitPrice"
FROM "PizzaOrderItems" poi
RIGHT JOIN pizza_orders po ON poi."OrderId" = po."OrderId"
RIGHT JOIN "Users" u ON po."UserId" = u."Id"
RIGHT JOIN "PizzaItems" pi ON poi."PizzaId" = pi."PizzaId";

/* SELECT 
   po."OrderId", po."OrderDate",
   u."Id" as UserId, u."Name", u."Email", u."Age",
   up."PhoneNumber", up."Address",
   poi."OrderItemId", poi."Quantity", poi."UnitPrice",
   pi."PizzaId", pi."Name" as PizzaName, pi."Size", pi."Price", pi."IsVegetarian"
FROM "pizza_orders" po
JOIN "Users" u ON po."UserId" = u."Id"
LEFT JOIN "UserProfiles" up ON u."Id" = up."UserId"
JOIN "PizzaOrderItems" poi ON po."OrderId" = poi."OrderId"
JOIN "PizzaItems" pi ON poi."PizzaId" = pi."PizzaId"
ORDER BY po."OrderDate", po."OrderId";
*/ 

SELECT * FROM "UserProfiles" WHERE "UserId" = '670dffdc-a4dd-47d2-8aac-be8bb00452e4';

SELECT u."Id", u."Name", u."Email", u."Age", up."UserId", up."PhoneNumber", up."Address"
FROM "Users" u
LEFT JOIN "UserProfiles" up ON u."Id" = up."UserId"
WHERE 1=1


SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';

/* SELECT
   po."OrderId", po."OrderDate",
   u."Id", u."Name", u."Email", u."Age",
   up."PhoneNumber", up."Address",
   poi."OrderItemId", poi."Quantity", poi."UnitPrice",
   p."PizzaId", p."Name", p."Size", p."Price", p."IsVegetarian"
FROM "pizza_orders" po
JOIN "Users" u ON po."UserId" = u."Id"
LEFT JOIN "UserProfiles" up ON u."Id" = up."UserId"
JOIN "PizzaOrderItems" poi ON po."OrderId" = poi."OrderId"
JOIN "PizzaItems" p ON poi."PizzaId" = p."PizzaId"
ORDER BY po."OrderDate", po."OrderId";
*/

CREATE TABLE payment_methods (
 payment_method_id SERIAL PRIMARY KEY,
 name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE payments (
 payment_id SERIAL PRIMARY KEY,
 order_id UUID NOT NULL,
 payment_method_id INT NOT NULL,
 amount NUMERIC(10,2) NOT NULL,
 payment_date TIMESTAMP NOT NULL DEFAULT NOW()
);

select * from payment_methods;
select * from payments;

ALTER TABLE payments
ADD COLUMN order_type VARCHAR(20);


SELECT p.*, po.*
FROM payments p
JOIN pizza_orders po ON p.order_id = po."OrderId"
WHERE p.order_type = 'pizza';



SELECT p.*, d.*
FROM payments p
JOIN drinks_orders d ON p.order_id = d."OrderId";


INSERT INTO payment_methods (name) VALUES
('Cash'),
('Card');

INSERT INTO payments (order_id, payment_method_id, amount, payment_date, order_type) VALUES
('f09686a5-9ea1-4023-8556-b2003fb16f4c', 1, 19.99, '2025-06-28 12:30:00', 'drink'),
('4adf9527-a452-4835-b036-d394ef8dafe7', 2, 34.50, '2025-06-28 13:15:00', 'drink'),
('b9a9d0b3-bb5d-464e-8b47-0580ef0f3414', 1, 12.00, '2025-03-20 13:15:00', 'pizza'),
('02f9b04e-6546-48a5-a06d-a04ae8f3ed87', 2, 25.75, '2025-04-18 12:30:00', 'pizza');

SELECT 
   p.order_id,
   p.amount,
   p.payment_date,
   p.order_type,
   po."UserId" AS pizza_user_id,
   d."UserId" AS drink_user_id
FROM "payments" p
LEFT JOIN pizza_orders po 
   ON p.order_id = po."OrderId" AND p.order_type = 'pizza'
LEFT JOIN drinks_orders d 
   ON p.order_id = d."OrderId" AND p.order_type = 'drink';


CREATE TABLE notifications (
   notification_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
   user_id UUID NOT NULL,
   message TEXT NOT NULL,
   is_read BOOLEAN NOT NULL DEFAULT FALSE,
   created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
   link TEXT NULL,
   is_deleted BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE INDEX idx_notifications_user_id ON notifications(user_id);

CREATE EXTENSION IF NOT EXISTS "pgcrypto";

select * from notifications;

SELECT * FROM notifications LIMIT 1;


ALTER TABLE notifications ADD COLUMN is_deleted BOOLEAN NOT NULL DEFAULT FALSE;
