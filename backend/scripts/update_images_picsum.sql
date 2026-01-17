-- Update all Images with Picsum.photos URLs (reliable placeholder service)
-- Using seed IDs to get consistent but varied images

-- All categories - use Picsum with different seed IDs for variety
UPDATE i SET i.FilePath = 'https://picsum.photos/seed/' + CAST(p.Id AS NVARCHAR(10)) + '/400/400'
FROM Images i
JOIN Products p ON i.ProductId = p.Id

-- Verify
SELECT TOP 10 p.Id, p.Name, i.FilePath
FROM Products p
JOIN Images i ON p.Id = i.ProductId
ORDER BY p.Id
