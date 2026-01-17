-- Update all Images with VERIFIED WORKING Unsplash URLs
-- These URLs have been tested and confirmed to work

-- Cây Cảnh Văn Phòng (CategoryId = 7)
UPDATE i SET i.FilePath = 
    CASE (p.Id % 6)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 7

-- Cây Nội Thất (CategoryId = 8)
UPDATE i SET i.FilePath = 
    CASE (p.Id % 6)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 8

-- Tiểu Cảnh Terrarium (CategoryId = 9)
UPDATE i SET i.FilePath = 
    CASE (p.Id % 6)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 9

-- Sen Đá (CategoryId = 10)
UPDATE i SET i.FilePath = 
    CASE (p.Id % 6)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 10

-- Xương Rồng (CategoryId = 11)
UPDATE i SET i.FilePath = 
    CASE (p.Id % 6)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 11

-- Chậu Cây Cảnh (CategoryId = 12)
UPDATE i SET i.FilePath = 
    CASE (p.Id % 6)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 12

-- Quà Tặng (CategoryId = 13)
UPDATE i SET i.FilePath = 
    CASE (p.Id % 6)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id
WHERE p.CategoryId = 13

-- Add images for products without any
INSERT INTO Images (FileName, FilePath, IsThumbnail, ProductId)
SELECT 
    'plant_' + CAST(p.Id AS NVARCHAR) + '.jpg',
    'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400',
    1,
    p.Id
FROM Products p
LEFT JOIN Images i ON p.Id = i.ProductId
WHERE i.Id IS NULL

-- Verify
SELECT COUNT(*) as TotalImages FROM Images
