-- Script to update Images table with PROPER plant-related images ONLY
-- No books, no cars, no gardening tools - only actual plants

-- First, let's check which products have NULL or empty FilePath
-- SELECT p.Id, p.Name, p.CategoryId, i.FilePath
-- FROM Products p
-- LEFT JOIN Images i ON p.Id = i.ProductId
-- WHERE i.FilePath IS NULL OR i.FilePath = ''

-- Cây Cảnh Văn Phòng (CategoryId = 7) - Office Plants - 8 different images
UPDATE i SET i.FilePath = 
    CASE (p.Id % 8)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1592150621744-aca64f48394a?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1604762524889-3e2fcc145683?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1620127807580-990c3ecebd14?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1597055181300-e3633a917e6a?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1612363148951-15f16817648f?w=400'
        WHEN 6 THEN 'https://images.unsplash.com/photo-1595594424235-7e1d8e79e6d1?w=400'
        WHEN 7 THEN 'https://images.unsplash.com/photo-1593691509543-c55fb32d8de5?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 7

-- Cây Nội Thất (CategoryId = 8) - Indoor/Houseplants - 8 different images
UPDATE i SET i.FilePath = 
    CASE (p.Id % 8)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1598880513969-279cb8322c81?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1617173944883-6ffbbec89b77?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1614594975525-e45190c55d0b?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1545241047-6083a3684587?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1598880513973-a33b7c8a0893?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1620803366004-119b57f54cd6?w=400'
        WHEN 6 THEN 'https://images.unsplash.com/photo-1551893134-c51c490f79a4?w=400'
        WHEN 7 THEN 'https://images.unsplash.com/photo-1593482892540-70673c4ac944?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 8

-- Tiểu Cảnh Terrarium (CategoryId = 9) - 8 different images
UPDATE i SET i.FilePath = 
    CASE (p.Id % 8)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1523575518836-b28d7d86a749?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1604762433068-0a7b3c3a0fc5?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1521334884684-d80222895322?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1612363148951-15f16817648f?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1518882605630-8b31a03e97eb?w=400'
        WHEN 6 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 7 THEN 'https://images.unsplash.com/photo-1590598016644-f3b1e48b0d38?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 9

-- Sen Đá (CategoryId = 10) - Succulents - 8 different images
UPDATE i SET i.FilePath = 
    CASE (p.Id % 8)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1520302519878-3769310c4e3c?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1523575518836-b28d7d86a749?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1604762433068-0a7b3c3a0fc5?w=400'
        WHEN 6 THEN 'https://images.unsplash.com/photo-1521334884684-d80222895322?w=400'
        WHEN 7 THEN 'https://images.unsplash.com/photo-1537039557108-4a42c334fd5e?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 10

-- Xương Rồng (CategoryId = 11) - Cactus - 8 different images
UPDATE i SET i.FilePath = 
    CASE (p.Id % 8)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1551893478-d726eaf0442c?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1515767166979-26ce4fc0f629?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1508022713622-df2d8a7d3f5b?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1549876506-85e0f7d22bdd?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1573666126940-a0836ca20e22?w=400'
        WHEN 6 THEN 'https://images.unsplash.com/photo-1551893134-c51c490f79a4?w=400'
        WHEN 7 THEN 'https://images.unsplash.com/photo-1545241047-6083a3684587?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 11

-- Quà Tặng (CategoryId = 13) - Gift Plants/Bouquets - 8 different images
UPDATE i SET i.FilePath = 
    CASE (p.Id % 8)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1592150621744-aca64f48394a?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1604762524889-3e2fcc145683?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1598880513969-279cb8322c81?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1617173944883-6ffbbec89b77?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1614594975525-e45190c55d0b?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1593691509543-c55fb32d8de5?w=400'
        WHEN 6 THEN 'https://images.unsplash.com/photo-1597055181300-e3633a917e6a?w=400'
        WHEN 7 THEN 'https://images.unsplash.com/photo-1620127807580-990c3ecebd14?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 13

-- Also update Chậu Cây Cảnh (CategoryId = 12) with proper plant pot images
UPDATE i SET i.FilePath = 
    CASE (p.Id % 8)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1592150621744-aca64f48394a?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1604762524889-3e2fcc145683?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1598880513969-279cb8322c81?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1617173944883-6ffbbec89b77?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1614594975525-e45190c55d0b?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1593691509543-c55fb32d8de5?w=400'
        WHEN 6 THEN 'https://images.unsplash.com/photo-1597055181300-e3633a917e6a?w=400'
        WHEN 7 THEN 'https://images.unsplash.com/photo-1620127807580-990c3ecebd14?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 12

-- Add images for products that don't have any image record
INSERT INTO Images (FileName, FilePath, IsThumbnail, ProductId)
SELECT 
    'product_' + CAST(p.Id AS NVARCHAR) + '.jpg',
    CASE (p.Id % 8)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1592150621744-aca64f48394a?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1604762524889-3e2fcc145683?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1598880513969-279cb8322c81?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1617173944883-6ffbbec89b77?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1614594975525-e45190c55d0b?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1593691509543-c55fb32d8de5?w=400'
        WHEN 6 THEN 'https://images.unsplash.com/photo-1597055181300-e3633a917e6a?w=400'
        WHEN 7 THEN 'https://images.unsplash.com/photo-1620127807580-990c3ecebd14?w=400'
    END,
    1,
    p.Id
FROM Products p
LEFT JOIN Images i ON p.Id = i.ProductId
WHERE i.Id IS NULL

-- Verify: check for any remaining problematic images
SELECT TOP 20 p.Name, p.CategoryId, i.FilePath
FROM Products p
JOIN Images i ON p.Id = i.ProductId
ORDER BY p.CategoryId, p.Id
