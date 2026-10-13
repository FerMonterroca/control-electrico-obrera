# Base de datos

## Modelo inicial

- `Brand` y `Category` organizan el catálogo. `Category` admite una relación padre-hijas.
- `Product` pertenece a una categoría y opcionalmente a una marca; contiene imágenes y especificaciones técnicas dinámicas.
- `Customer` es una ficha interna sin login y conserva sus solicitudes de cotización y material especial.
- `QuoteRequest` contiene partidas (`QuoteItem`) y metadatos de adjuntos (`QuoteAttachment`).
- `SpecialMaterialRequest` registra búsquedas manuales y sus metadatos de adjuntos.
- `AdminUser` está modelado para una fase futura, sin login, hashing, tokens ni credenciales sembradas.
- `SiteSettings` contiene la configuración básica del negocio. La regla de un único registro lógico se aplicará cuando exista el flujo de administración; no hay seed en esta fase.

```text
Brand ──< Product >── Category ──< Category
                  ├──< ProductImage
                  ├──< ProductSpecification
                  └──< QuoteItem

Customer ──< QuoteRequest ──< QuoteItem
         │               └──< QuoteAttachment
         └──< SpecialMaterialRequest ──< SpecialMaterialAttachment
```

## Decisiones de tipos

- Las claves son `bigint` autogeneradas (`long` en C#).
- Cantidades comerciales usan `numeric(18,3)`, nunca punto flotante.
- `NeededByDate` usa `date`/`DateOnly` porque no representa una hora.
- `CreatedAt`, `UpdatedAt` y `LastLoginAt` usan `timestamp with time zone`/`DateTimeOffset`. Las entidades inicializan timestamps con `UtcNow` y la base aporta `CURRENT_TIMESTAMP` como valor por defecto. Los futuros casos de escritura deberán actualizar `UpdatedAt` explícitamente; no existe infraestructura de auditoría oculta.
- Descripciones y notas largas usan `text`; identificadores y metadatos tienen límites explícitos.

## Enums

Los enums se almacenan como enteros mediante conversiones explícitas de EF Core. Esto evita strings mágicos y mantiene esta primera migración sencilla. Los valores numéricos son estables y no deben reordenarse:

- `ProductAvailabilityStatus`: `Consult`, `AvailableOnRequest`, `DiscontinuedSearch`, `Unavailable`.
- `QuoteStatus`: `New`, `UnderReview`, `PreparingQuote`, `Quoted`, `FollowUp`, `Closed`, `Cancelled`.
- `QuoteSource`: `Catalog`, `MaterialList`, `Companies`, `SpecialMaterial`, `Admin`.
- `SpecialMaterialRequestStatus`: `New`, `Searching`, `Found`, `Quoted`, `NotFound`, `Closed`.
- `AdminRole`: `Owner`, `Admin`.

`Urgency` permanece como texto limitado a 50 caracteres porque todavía no se ha definido un vocabulario de negocio estable para convertirlo en enum.

## Historial de cotizaciones

`QuoteRequest` y `SpecialMaterialRequest` conservan `CustomerId` para mantener la relación con la ficha interna y permitir navegación, seguimiento y futura deduplicación. Además guardan snapshots de nombre, teléfono, email y empresa para que una edición posterior del cliente no altere el contacto histórico de la solicitud.

`QuoteItem` guarda nombre, marca, modelo y SKU/código como snapshots. El código o part number puede ser determinante para identificar material eléctrico incluso si el nombre comercial cambia. `ProductId` es nullable y usa `SET NULL`: una partida puede representar un material capturado manualmente y continúa siendo legible si el producto relacionado se elimina.

Todos los valores snapshot representan el estado al momento de crear la solicitud. Una vez creada, no deben sincronizarse ni sobrescribirse cuando cambien `Customer` o `Product`.

## Adjuntos

`ProductImage`, `QuoteAttachment` y `SpecialMaterialAttachment` almacenan únicamente metadatos y `StorageKey`. PostgreSQL no almacena el archivo binario. Una fase posterior resolverá esa clave contra almacenamiento S3-compatible.

## Delete behaviors

Usan `CASCADE` solo los registros que no tienen sentido fuera de su agregado:

- `Product` → imágenes y especificaciones.
- `QuoteRequest` → partidas y adjuntos.
- `SpecialMaterialRequest` → adjuntos.

Las relaciones que preservan catálogo o historial usan protección explícita:

- Marca → productos: `RESTRICT`.
- Categoría → productos: `RESTRICT`.
- Categoría padre → subcategorías: `RESTRICT`.
- Cliente → cotizaciones y solicitudes especiales: `RESTRICT`.
- Producto → partidas históricas: `SET NULL`.

Productos, marcas y categorías deben desactivarse con `IsActive` en el flujo normal.

## Índices

Son únicos los slugs de producto, marca y categoría, el email administrativo y el folio de cotización. También existen índices para nombre, marca, categoría y estado activo de productos; estado y fecha de cotizaciones; estado de solicitudes especiales; claves foráneas; y orden de imágenes/especificaciones por producto.

## Migraciones

Restaura la herramienta local una vez:

```powershell
cd backend
dotnet tool restore
```

Carga la cadena de conexión y crea una migración futura:

```powershell
$env:ConnectionStrings__Default = (Get-Content ..\.env | Where-Object { $_ -match '^ConnectionStrings__Default=' }) -replace '^ConnectionStrings__Default=', ''
dotnet tool run dotnet-ef migrations add DescriptiveMigrationName --output-dir Migrations
```

Revisa siempre los archivos generados antes de aplicarlos. Para actualizar la base:

```powershell
dotnet tool run dotnet-ef database update
```

Comprueba que modelo y migraciones estén sincronizados:

```powershell
dotnet tool run dotnet-ef migrations has-pending-model-changes
```
