-- First, let's delete existing users and create new ones properly
-- This ensures the password hash is generated correctly

DELETE FROM Users WHERE Name IN ('admin', 'user', 'testuser');
