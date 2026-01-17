-- Update admin role
UPDATE Users SET Role = 'Admin' WHERE Name = 'admin'

-- Check users
SELECT Id, Name, FullName, Role FROM Users
