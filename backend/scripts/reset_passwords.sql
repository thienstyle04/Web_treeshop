-- Đặt mật khẩu mới cho tất cả user
-- Password hash được tạo bằng BCrypt cho mật khẩu: "123456"

-- Hash của "123456" = $2a$11$K10EBd0BYK3qG5Uy7qVKEeV9CVi4vU1b1qJzNHCIUvlYYqQ7.4Qo2

UPDATE Users SET PasswordHash = '$2a$11$K10EBd0BYK3qG5Uy7qVKEeV9CVi4vU1b1qJzNHCIUvlYYqQ7.4Qo2' WHERE Name = 'admin'
UPDATE Users SET PasswordHash = '$2a$11$K10EBd0BYK3qG5Uy7qVKEeV9CVi4vU1b1qJzNHCIUvlYYqQ7.4Qo2' WHERE Name = 'user'
UPDATE Users SET PasswordHash = '$2a$11$K10EBd0BYK3qG5Uy7qVKEeV9CVi4vU1b1qJzNHCIUvlYYqQ7.4Qo2' WHERE Name = 'testuser'

SELECT Name, Role, 'Password: 123456' as Note FROM Users
