DROP TABLE IF EXISTS Expenses;
DROP TABLE IF EXISTS Categories;

CREATE TABLE Categories (
-- 	Identity takes two parameters. (Seed, Increment)
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Name VARCHAR(50) NOT NULL UNIQUE
);


CREATE TABLE Expenses(
    -- NEWSEQUENTIALID is a safer version of NewID() for distributed databases.
	Id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
-- 	nvarchar stores unicode while varchar only stores ascii letter.
	Description NVARCHAR(200) NOT NULL,
	Amount DECIMAL(18, 2) NOT NULL,
	CreatedDate DATETIME2(0) DEFAULT SYSUTCDATETIME(),
	CategoryId INT NOT NULL REFERENCES Categories(Id)

);

INSERT INTO Categories(Name)
VALUES
	('Food'),
	('Transport'),
	('Utilities');


INSERT INTO Expenses(Description, Amount, CategoryId)
VALUES
	('Ate KFC', 56.33, 1),
	('Uber to Uni', 14.5, 2),
    ('Mobile SIM Recharge', 25.5, 3);

SELECT * FROM Expenses;
SELECT * FROM Categories;

-- query to obtain the total expense along with their category name
SELECT e.Description AS description, e.Amount AS amount, c.Name AS Category FROM Expenses e INNER JOIN Categories c ON e.CategoryId = c.Id;

-- query to obtain the total expense grouped by the category
SELECT c.Name, SUM(e.Amount) AS TotalExpense FROM Expenses e INNER JOIN Categories c on e.CategoryId = c.Id GROUP BY c.Name;
