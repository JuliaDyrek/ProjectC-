CREATE USER 'user'@'localhost' IDENTIFIED BY 'haslohaslo123';

CREATE DATABASE restaurant;

GRANT ALL PRIVILEGES ON restaurant.* TO 'user'@'localhost';
FLUSH PRIVILEGES;

USE restaurant;

INSERT INTO dish (name, price) VALUES
('Spaghetti Carbonara', 12.99),
('Chicken Caesar Salad', 10.50),
('Vegetable Stir-fry', 8.75),
('Margherita Pizza', 9.95),
('Beef Tacos', 7.99),
('Grilled Salmon', 15.30),
('Cheeseburger', 11.00),
('Fish and Chips', 13.20),
('Vegan Burrito', 9.50),
('Pasta Primavera', 11.75),
('Buffalo Wings', 6.99),
('Eggplant Parmesan', 12.25),
('Peking Duck', 18.50),
('Lamb Curry', 14.95),
('Chicken Schnitzel', 13.80),
('Lasagna', 13.00),
('Pulled Pork Sandwich', 9.99),
('Shrimp Scampi', 16.00),
('Beef Wellington', 25.00),
('Vegetable Samosas', 5.50);