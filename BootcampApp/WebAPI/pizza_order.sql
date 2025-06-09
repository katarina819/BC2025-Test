CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE "Users" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(100) NOT NULL
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


INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Ana', 'ana@example.com') 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Ivan', 'ivan@example.com') 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Marko', 'marko@example.com') 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Petra', 'petra@example.com') 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Luka', 'luka@example.com') 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Maja', 'maja@example.com') 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Nikola', 'nikola@example.com') 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Tina', 'tina@example.com') 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Filip', 'filip@example.com') 
RETURNING "Id";

INSERT INTO "Users" ("Id", "Name", "Email") 
VALUES (uuid_generate_v4(), 'Ivana', 'ivana@example.com') 
RETURNING "Id";

select * from "Users";

INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('1fb4eecd-2ca7-472e-96bf-9d228a49836d', 'Ana', 'ana@example.com');
INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('daa54377-0a81-4d9f-aa5f-e2fe3d2cf24b', 'Ivan', 'ivan@example.com');
INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('e54cee70-3e5e-4e7c-a95c-780b58cd5926', 'Marko', 'marko@example.com');
INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('69ae3bb2-781c-4e0c-b1a7-b662d65e80b2', 'Petra', 'petra@example.com');
INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('e5e7b3bc-1986-47e3-a28d-ec8b739223b6', 'Luka', 'luka@example.com');
INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('a4836e85-c454-4ec0-b1f4-4fe31cc5eea2', 'Maja', 'maja@example.com');
INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('011c1f93-abd2-43e2-9634-cb24decff7e4', 'Nikola', 'nikola@example.com');
INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('7beb4278-9cba-42c6-b1d8-765f388810ad', 'Tina', 'tina@example.com');
INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('3f5bc139-40f5-4716-8feb-34faefa05bd5', 'Filip', 'filip@example.com');
INSERT INTO "Users" ("Id", "Name", "Email") VALUES ('c51bcdbf-076a-48ac-b70a-91f60fe48945', 'Ivana', 'ivana@example.com');


CREATE TABLE "UserProfiles" (
    "UserId" UUID PRIMARY KEY,
    "PhoneNumber" VARCHAR(20),
    "Address" TEXT,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

INSERT INTO "UserProfiles" ("UserId", "PhoneNumber", "Address") VALUES
('02705186-7608-4e49-bd0e-450e7253735c', '+385981112233', 'Vukovarska 12, Osijek'),         -- Ana
('670dffdc-a4dd-47d2-8aac-be8bb00452e4', '+385981234567', 'Europske avenije 34, Osijek'),   -- Ivan
('81ee6d58-4a84-4f0b-8539-565e4b55ad07', '+385997776655', 'J. J. Strossmayera 5, Osijek'),   -- Marko
('72926c3a-855f-45d9-812a-e8c8faafeecf', '+385912223344', 'Kapucinska 88, Osijek'),         -- Petra
('89a819e1-ac2a-41e8-98ff-516d1b994b87', '+385923334455', 'Radićeva 9, Osijek'),            -- Luka
('6bc630dc-e0b2-4ae7-a9ae-77e9184a9ee1', '+385994445566', 'Županijska 77, Osijek'),         -- Maja
('733ac241-17f1-450d-91a5-e7827c53729d', '+385995556677', 'Trg slobode 13, Osijek'),        -- Nikola
('d0456ebf-31c2-4b87-aa1d-087b42e8a0f6', '+385987878787', 'Sjenjak 2, Osijek'),             -- Tina
('f26edee6-6edb-42ae-97f5-00c270e0b48b', '+385996969696', 'Retfalačka 4, Osijek'),          -- Filip
('f8ef7302-39f8-4ef7-b6bc-231619095579', '+385991112233', 'Gornjodravska obala 5, Osijek'); -- Ivana

select * from "UserProfiles"; 

CREATE TABLE "PizzaItems" (
    "PizzaId" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(100) NOT NULL,
    "Size" VARCHAR(20),  -- npr. "Mala", "Srednja", "Velika"
    "Price" NUMERIC(10, 2) NOT NULL
);

INSERT INTO "PizzaItems" ("Name", "Size", "Price") VALUES
('Margherita', 'Srednja', 6.50),
('Capricciosa', 'Velika', 8.50),
('Pepperoni', 'Srednja', 7.80),
('Vegetariana', 'Mala', 5.90),
('Quattro Formaggi', 'Velika', 9.20);

ALTER TABLE "PizzaItems"
ADD COLUMN "IsVegetarian" BOOLEAN;


UPDATE "PizzaItems"
SET "IsVegetarian" = TRUE
WHERE "Name" IN ('Margherita', 'Vegetariana', 'Quattro Formaggi');

UPDATE "PizzaItems"
SET "IsVegetarian" = FALSE
WHERE "Name" IN ('Capricciosa', 'Pepperoni');

SELECT * FROM "PizzaItems";

CREATE TABLE "PizzaOrders" (
    "OrderId" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UserId" UUID NOT NULL,
    "OrderDate" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

INSERT INTO "PizzaOrders" ("UserId") VALUES
('8545b460-fec0-46ce-af3b-3b0f4644c699'),  -- Ana
('670dffdc-a4dd-47d2-8aac-be8bb00452e4'),  -- Ivan
('81ee6d58-4a84-4f0b-8539-565e4b55ad07');  -- Marko

select * from "PizzaOrders";

CREATE TABLE "PizzaOrderItems" (
    "OrderItemId" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "OrderId" UUID NOT NULL,
    "PizzaId" UUID NOT NULL,
    "Quantity" INT NOT NULL,
    "UnitPrice" NUMERIC(10, 2) NOT NULL,
    FOREIGN KEY ("OrderId") REFERENCES "PizzaOrders"("OrderId") ON DELETE CASCADE,
    FOREIGN KEY ("PizzaId") REFERENCES "PizzaItems"("PizzaId") ON DELETE CASCADE
);

INSERT INTO "PizzaOrderItems" ("OrderId", "PizzaId", "Quantity", "UnitPrice") VALUES
('80bfbab0-a2e9-4581-962d-6a89f6343279', 'd48b7155-4f17-48be-a8b1-215b1ee2c252', 2, 6.50),
('80bfbab0-a2e9-4581-962d-6a89f6343279', 'cd1ba09e-9260-4b19-ba69-97746033bd09', 1, 7.80),
('b9a9d0b3-bb5d-464e-8b47-0580ef0f3414', '61b508be-9f44-445b-8acf-95fa053458d0', 3, 9.20),
('02f9b04e-6546-48a5-a06d-a04ae8f3ed87', 'ccbbc48f-f38a-4446-886f-27a5d457a45c', 1, 8.50),
('02f9b04e-6546-48a5-a06d-a04ae8f3ed87', '75ce1adc-73ba-40d5-ab45-760567955712', 2, 5.90);

select * from "PizzaOrderItems";

-- SELECT "OrderId", "UserId", "OrderDate" FROM "PizzaOrders";
-- SELECT "PizzaId", "Name", "Size" FROM "PizzaItems";

--inner join
SELECT 
    po."OrderId",
    u."Name" AS "UserName",
    pi."Name" AS "PizzaName",
    poi."Quantity",
    poi."UnitPrice"
FROM "PizzaOrders" po
INNER JOIN "Users" u ON po."UserId" = u."Id"
INNER JOIN "PizzaOrderItems" poi ON po."OrderId" = poi."OrderId"
INNER JOIN "PizzaItems" pi ON poi."PizzaId" = pi."PizzaId";

--left join
SELECT 
    po."OrderId",
    u."Name" AS "UserName",
    pi."Name" AS "PizzaName",
    poi."Quantity",
    poi."UnitPrice"
FROM "PizzaOrders" po
LEFT JOIN "Users" u ON po."UserId" = u."Id"
LEFT JOIN "PizzaOrderItems" poi ON po."OrderId" = poi."OrderId"
LEFT JOIN "PizzaItems" pi ON poi."PizzaId" = pi."PizzaId";

--right join
SELECT 
    po."OrderId",
    u."Name" AS "UserName",
    pi."Name" AS "PizzaName",
    poi."Quantity",
    poi."UnitPrice"
FROM "PizzaOrderItems" poi
RIGHT JOIN "PizzaOrders" po ON poi."OrderId" = po."OrderId"
RIGHT JOIN "Users" u ON po."UserId" = u."Id"
RIGHT JOIN "PizzaItems" pi ON poi."PizzaId" = pi."PizzaId";



