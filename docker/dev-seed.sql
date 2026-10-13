-- Development-only catalog data. This script is idempotent and never deletes data.
BEGIN;

INSERT INTO "Brands" ("Name", "Slug", "IsActive")
VALUES
    ('Schneider Electric', 'schneider-electric', TRUE),
    ('IUSA', 'iusa', TRUE),
    ('Inactive Development Brand', 'inactive-development-brand', FALSE)
ON CONFLICT ("Slug") DO NOTHING;

INSERT INTO "Categories" ("Name", "Slug", "IsActive")
VALUES
    ('Contactores', 'contactores', TRUE),
    ('Protección eléctrica', 'proteccion-electrica', TRUE),
    ('Inactive Development Category', 'inactive-development-category', FALSE)
ON CONFLICT ("Slug") DO NOTHING;

INSERT INTO "Products" (
    "Name",
    "Slug",
    "ShortDescription",
    "BrandId",
    "CategoryId",
    "Model",
    "SkuOrCode",
    "Unit",
    "AvailabilityStatus",
    "IsFeatured",
    "IsActive")
VALUES
    (
        'Contactor industrial de prueba 18 A',
        'contactor-prueba-18a',
        'Producto ficticio para desarrollo y pruebas de la API.',
        (SELECT "Id" FROM "Brands" WHERE "Slug" = 'schneider-electric'),
        (SELECT "Id" FROM "Categories" WHERE "Slug" = 'contactores'),
        'DEV-LC18',
        'DEV-CTR-018',
        'piece',
        0,
        TRUE,
        TRUE
    ),
    (
        'Contactor industrial de prueba 32 A',
        'contactor-prueba-32a',
        'Producto ficticio para probar filtros de catálogo.',
        (SELECT "Id" FROM "Brands" WHERE "Slug" = 'schneider-electric'),
        (SELECT "Id" FROM "Categories" WHERE "Slug" = 'contactores'),
        'DEV-LC32',
        'DEV-CTR-032',
        'piece',
        1,
        FALSE,
        TRUE
    ),
    (
        'Interruptor termomagnético de prueba',
        'interruptor-prueba-20a',
        'Producto ficticio; no representa inventario real.',
        (SELECT "Id" FROM "Brands" WHERE "Slug" = 'iusa'),
        (SELECT "Id" FROM "Categories" WHERE "Slug" = 'proteccion-electrica'),
        'DEV-QO20',
        'DEV-BRK-020',
        'piece',
        1,
        TRUE,
        TRUE
    ),
    (
        'Repuesto discontinuado de prueba',
        'repuesto-discontinuado-prueba',
        'Producto ficticio para probar búsqueda de material discontinuado.',
        (SELECT "Id" FROM "Brands" WHERE "Slug" = 'schneider-electric'),
        (SELECT "Id" FROM "Categories" WHERE "Slug" = 'proteccion-electrica'),
        'DEV-OLD-01',
        'DEV-DIS-001',
        'piece',
        2,
        FALSE,
        TRUE
    ),
    (
        'Producto inactivo de prueba',
        'producto-inactivo-prueba',
        'Este producto no debe aparecer en la API pública.',
        (SELECT "Id" FROM "Brands" WHERE "Slug" = 'schneider-electric'),
        (SELECT "Id" FROM "Categories" WHERE "Slug" = 'contactores'),
        'DEV-HIDDEN',
        'DEV-HIDDEN-001',
        'piece',
        0,
        FALSE,
        FALSE
    )
ON CONFLICT ("Slug") DO NOTHING;

INSERT INTO "ProductImages" (
    "ProductId",
    "StorageKey",
    "AltText",
    "SortOrder",
    "IsPrimary")
SELECT
    product."Id",
    'development/products/contactor-prueba-18a.webp',
    'Imagen ficticia del contactor de prueba',
    0,
    TRUE
FROM "Products" AS product
WHERE product."Slug" = 'contactor-prueba-18a'
  AND NOT EXISTS (
      SELECT 1
      FROM "ProductImages" AS image
      WHERE image."ProductId" = product."Id"
        AND image."StorageKey" = 'development/products/contactor-prueba-18a.webp'
  );

INSERT INTO "ProductSpecifications" (
    "ProductId",
    "Name",
    "Value",
    "Unit",
    "SortOrder")
SELECT
    product."Id",
    'Current',
    '18',
    'A',
    0
FROM "Products" AS product
WHERE product."Slug" = 'contactor-prueba-18a'
  AND NOT EXISTS (
      SELECT 1
      FROM "ProductSpecifications" AS specification
      WHERE specification."ProductId" = product."Id"
        AND specification."Name" = 'Current'
  );

COMMIT;
