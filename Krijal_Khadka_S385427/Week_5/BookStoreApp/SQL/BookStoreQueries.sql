USE BookStoreDB_Dev;
GO

-- Display existing data
SELECT * FROM Books;
SELECT * FROM Printers;
SELECT * FROM Scanners;
GO

-- Check migration history
SELECT *
FROM __EFMigrationsHistory;
GO

-- Practise CRUD using a temporary transaction
BEGIN TRANSACTION;

-- CREATE
INSERT INTO Books (Name)
VALUES ('Learning SQL');

-- READ
SELECT *
FROM Books
WHERE Name = 'Learning SQL';

-- UPDATE
UPDATE Books
SET Name = 'Learning SQL Server'
WHERE Name = 'Learning SQL';

-- Confirm the update
SELECT *
FROM Books
WHERE Name = 'Learning SQL Server';

-- DELETE
DELETE FROM Books
WHERE Name = 'Learning SQL Server';

-- Undo the practice changes
ROLLBACK TRANSACTION;
GO