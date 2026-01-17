-- Use only CONFIRMED WORKING Unsplash URLs from screenshots
-- These specific URLs were seen loading correctly

-- List of 6 verified working plant image URLs:
-- 1. https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400  (succulent in teal pot)
-- 2. https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400  (cactus)
-- 3. https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400  (small plant)
-- 4. https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400  (succulent)
-- 5. https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400  (monstera)  
-- 6. https://images.unsplash.com/photo-1520302519878-3769310c4e3c?w=400  (plant)

-- Update all product images using only these verified URLs
UPDATE i SET i.FilePath = 
    CASE (p.Id % 6)
        WHEN 0 THEN 'https://images.unsplash.com/photo-1509423350716-97f9360b4e09?w=400'
        WHEN 1 THEN 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400'
        WHEN 2 THEN 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400'
        WHEN 3 THEN 'https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400'
        WHEN 4 THEN 'https://images.unsplash.com/photo-1446071103084-c257b5f70672?w=400'
        WHEN 5 THEN 'https://images.unsplash.com/photo-1520302519878-3769310c4e3c?w=400'
    END
FROM Images i
JOIN Products p ON i.ProductId = p.Id

-- Verify
SELECT COUNT(*) as UpdatedCount FROM Images WHERE FilePath IS NOT NULL
