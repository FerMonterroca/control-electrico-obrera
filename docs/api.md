# API pública del catálogo

Base local: `http://localhost:5000`. Todos los endpoints son de solo lectura y excluyen registros inactivos. Los enums se serializan por nombre, por ejemplo `"Consult"`.

## Productos

### `GET /api/products`

Devuelve una respuesta paginada:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 24,
  "totalItems": 0,
  "totalPages": 0
}
```

Query params:

| Parámetro | Comportamiento |
|---|---|
| `page` | Entero de 1 a 1,000,000. Default: 1. |
| `pageSize` | Entero de 1 a 100. Default: 24. |
| `search` | Busca sin distinguir mayúsculas en nombre, modelo, SKU/código y nombre de marca. Máximo 200 caracteres. |
| `brand` | Slug exacto de marca. |
| `category` | Slug exacto de categoría. No incluye descendientes. |
| `availability` | `Consult`, `AvailableOnRequest`, `DiscontinuedSearch` o `Unavailable`. No distingue mayúsculas. |
| `featured` | `true` o `false`. |
| `sort` | `name-asc`, `name-desc` o `newest`. Default: `newest`. |

Ejemplos:

```text
GET /api/products?page=1&pageSize=24
GET /api/products?search=DEV-CTR
GET /api/products?brand=schneider-electric
GET /api/products?category=contactores
GET /api/products?availability=Consult&featured=true
GET /api/products?sort=name-asc
```

Los productos asociados a una marca o categoría inactiva tampoco aparecen públicamente.

### `GET /api/products/{slug}`

Busca por slug exacto. Devuelve marca, categoría, metadatos SEO, imágenes ordenadas por `SortOrder` y especificaciones ordenadas por `SortOrder`. Mientras no exista almacenamiento S3, las imágenes exponen `storageKey`, no una URL pública.

Devuelve `404` si el producto no existe, está inactivo o pertenece a una marca/categoría inactiva.

Ejemplo parcial de respuesta:

```json
{
  "id": 1,
  "name": "Contactor industrial de prueba 18 A",
  "slug": "contactor-prueba-18a",
  "brand": {
    "id": 1,
    "name": "Schneider Electric",
    "slug": "schneider-electric"
  },
  "availabilityStatus": "Consult",
  "images": [
    {
      "storageKey": "development/products/contactor-prueba-18a.webp",
      "altText": "Imagen ficticia del contactor de prueba",
      "sortOrder": 0,
      "isPrimary": true
    }
  ],
  "specifications": [
    {
      "name": "Current",
      "value": "18",
      "unit": "A",
      "sortOrder": 0
    }
  ]
}
```

## Marcas

### `GET /api/brands`

Lista marcas activas en orden alfabético como objetos `{ id, name, slug }`.

### `GET /api/brands/{slug}`

Devuelve el detalle de una marca activa o `404`.

## Categorías

### `GET /api/categories`

Devuelve una lista plana de categorías activas ordenadas alfabéticamente. Cada elemento contiene `parentCategoryId` cuando su categoría padre también está activa. La lista plana mantiene simple la navegación y evita imponer un árbol al consumidor.

### `GET /api/categories/{slug}`

Devuelve detalle, padre activo y subcategorías activas, o `404`.

## Errores

Errores de query y recursos inexistentes usan un sobre consistente:

```json
{
  "error": {
    "code": "invalid_availability",
    "message": "The requested availability value is invalid."
  }
}
```

- `400`: query inválida (`invalid_page`, `invalid_page_size`, `invalid_availability`, `invalid_featured`, `invalid_sort`, `invalid_search` o `invalid_filter`).
- `404`: recurso inexistente o inactivo.
- `500`: `internal_error`, sin stack trace ni información interna.
