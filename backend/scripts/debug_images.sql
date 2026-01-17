-- Find all images that still have problematic URLs (books, tools, cars)
SELECT p.Id, p.Name, p.CategoryId, i.Id as ImageId, i.FilePath
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE 
    i.FilePath LIKE '%696a485928c7%'  -- books
    OR i.FilePath LIKE '%8113da705763%'  -- car
    OR i.FilePath LIKE '%scissors%'
    OR i.FilePath LIKE '%tools%'
    OR i.FilePath LIKE '%shovel%'
    OR i.FilePath LIKE '%book%'
ORDER BY p.Name

-- Check specific problematic products by name
SELECT p.Id, p.Name, i.Id as ImageId, i.FilePath
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.Name IN (
    N'Cây Cẩm Nhung',
    N'Cây Cần Thăng',
    N'Tiểu Cảnh Tết',
    N'Cây Thường Xuân',
    N'Set Văn Phòng',
    N'Cây Thẻ Bài Hồng'
)
ORDER BY p.Name

-- Check all existing FilePaths for variety
SELECT DISTINCT FilePath, COUNT(*) as cnt
FROM Images
GROUP BY FilePath
ORDER BY cnt DESC
