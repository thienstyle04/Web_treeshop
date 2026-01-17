-- Fix specific problematic products by name

-- First, find all products with problematic images and check what's happening
SELECT p.Id, p.Name, p.CategoryId, i.Id as ImageId, i.FilePath
FROM Products p
LEFT JOIN Images i ON p.Id = i.ProductId
WHERE p.Name LIKE N'%Cẩm%' 
   OR p.Name LIKE N'%Quà Sinh%'
   OR p.Name LIKE N'%Set Văn Phòng%'
   OR p.Name LIKE N'%Se Quà Cưới%'
   OR p.Name LIKE N'%Set Quà%'
   OR i.FilePath LIKE '%696a485928c7%'  -- books image
   OR i.FilePath LIKE '%8113da705763%'  -- car image  
   OR i.FilePath IS NULL

-- Fix Cây Cẩm Nhung (books image) - update to proper plant image
UPDATE i SET i.FilePath = 'https://images.unsplash.com/photo-1614594975525-e45190c55d0b?w=400'
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.Name LIKE N'%Cẩm Nhung%'

-- Fix Quà Sinh Nhật (shovel image) - update to proper plant image
UPDATE i SET i.FilePath = 'https://images.unsplash.com/photo-1592150621744-aca64f48394a?w=400'
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.Name LIKE N'%Quà Sinh Nhật%'

-- Fix all "Set" products that have broken or no images
UPDATE i SET i.FilePath = 'https://images.unsplash.com/photo-1604762524889-3e2fcc145683?w=400'
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.Name LIKE N'%Set Văn Phòng%'

UPDATE i SET i.FilePath = 'https://images.unsplash.com/photo-1617173944883-6ffbbec89b77?w=400'
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.Name LIKE N'%Set Quà Cưới%'

UPDATE i SET i.FilePath = 'https://images.unsplash.com/photo-1598880513969-279cb8322c81?w=400'
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.Name LIKE N'%Tiểu Cảnh Pha Lê%'

-- Remove all remaining problematic images (books, car, gardening tools)
UPDATE i SET i.FilePath = 'https://images.unsplash.com/photo-1592150621744-aca64f48394a?w=400'
FROM Images i
WHERE i.FilePath LIKE '%696a485928c7%'  -- books

UPDATE i SET i.FilePath = 'https://images.unsplash.com/photo-1604762524889-3e2fcc145683?w=400'
FROM Images i
WHERE i.FilePath LIKE '%8113da705763%'  -- car (Audi)

UPDATE i SET i.FilePath = 'https://images.unsplash.com/photo-1617173944883-6ffbbec89b77?w=400'
FROM Images i
WHERE i.FilePath LIKE '%tools%' OR i.FilePath LIKE '%shovel%'

-- Also fix any remaining NULL or empty FilePaths
UPDATE i SET i.FilePath = 'https://images.unsplash.com/photo-1592150621744-aca64f48394a?w=400'
FROM Images i
WHERE i.FilePath IS NULL OR i.FilePath = ''

-- Insert image records for products that don't have ANY image record
INSERT INTO Images (FileName, FilePath, IsThumbnail, ProductId)
SELECT 
    'plant_' + CAST(p.Id AS NVARCHAR) + '.jpg',
    'https://images.unsplash.com/photo-1592150621744-aca64f48394a?w=400',
    1,
    p.Id
FROM Products p
LEFT JOIN Images i ON p.Id = i.ProductId
WHERE i.Id IS NULL

-- Verify the fixes
SELECT p.Id, p.Name, p.CategoryId, i.FilePath
FROM Products p
LEFT JOIN Images i ON p.Id = i.ProductId
WHERE p.Name LIKE N'%Cẩm%' OR p.Name LIKE N'%Set%' OR p.Name LIKE N'%Quà%'
ORDER BY p.CategoryId, p.Id
