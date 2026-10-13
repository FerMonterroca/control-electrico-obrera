# Frontend público

## Alcance

La fase pública permite explorar el catálogo y contactar a Control Eléctrico Obrera. No contiene autenticación, administración, escritura en la API, carrito, cotizaciones persistidas, precios, stock, pagos ni uploads.

## Rutas

| Ruta | Renderizado | Datos |
|---|---|---|
| `/` | Bajo demanda | Categorías y productos destacados |
| `/catalogo` | Bajo demanda | Productos, marcas y categorías |
| `/productos/[slug]` | Bajo demanda | Detalle de producto |
| `/marcas` y `/marcas/[slug]` | Bajo demanda | Listado, detalle y productos exactos de marca |
| `/categorias` y `/categorias/[slug]` | Bajo demanda | Listado, detalle, subcategorías y productos de categoría exacta |
| `/empresas`, `/nosotros`, `/contacto`, `/preguntas-frecuentes` | Prerender | Contenido confirmado del negocio |
| `/404` | Prerender | Navegación de recuperación |

Astro usa `output: server` con `@astrojs/node`. Sólo las páginas que declaran `prerender = true` se generan de forma estática. Esta estrategia evita una SPA y entrega el catálogo como HTML inicial mientras mantiene actualizados los datos.

## Organización

- `src/layouts/BaseLayout.astro`: metadatos base, header, footer y salto al contenido.
- `src/components/`: navegación, footer, tarjetas y grid de productos.
- `src/lib/api.ts`: transporte HTTP y error tipado.
- `src/lib/api/catalog.ts`: métodos de lectura del catálogo.
- `src/lib/catalog.ts`: parámetros, disponibilidad, paginación y resolución futura de media.
- `src/lib/business.ts`: configuración temporal del negocio y helper de WhatsApp.
- `src/types/catalog.ts`: contratos TypeScript equivalentes a los DTOs públicos.
- `src/styles/global.css`: sistema visual, responsive y estados de foco.

## API y query params

`catalogApi` es la única puerta de entrada a los endpoints públicos. `/catalogo` usa un formulario HTML `GET`; `search`, `brand`, `category`, `availability`, `featured`, `sort` y `page` permanecen en la URL. `pageSize` también se conserva si un consumidor lo proporciona, aunque la UI usa 12 elementos por defecto. `parseCatalogQuery` acepta únicamente valores soportados y restablece página inválida a 1. Cambiar filtros reinicia la página porque el formulario no incluye `page`; los enlaces de paginación conservan el resto de los parámetros.

La búsqueda se envía completa a `GET /api/products`. El frontend no replica la lógica de nombre, modelo, SKU/código o marca.

Los errores de red/API muestran mensajes públicos sin JSON ni stack traces. Un detalle inexistente responde con status HTTP 404 y una salida navegable. Los resultados vacíos tienen un estado dedicado.

## Configuración del negocio y WhatsApp

`businessConfig` centraliza nombre, teléfono, número de WhatsApp, ubicación, giro y el texto sobre recepción de solicitudes. Es configuración frontend temporal hasta que exista `SiteSettings` administrable. `createWhatsAppUrl` usa el número normalizado y codifica el mensaje; el detalle de producto agrega nombre y modelo cuando existe.

## Imágenes

La API entrega `StorageKey`, no URLs públicas. `resolveMediaUrl(storageKey)` es el punto de extensión para el futuro almacenamiento S3-compatible y actualmente siempre devuelve `null`. Tarjetas y detalle muestran un fallback accesible; no se inventan dominios ni rutas públicas.

## Astro y React

Todo el frontend público está compuesto con Astro y HTML nativo. Búsqueda, filtros, paginación, menú móvil y FAQ funcionan sin JavaScript de aplicación. React permanece sólo en `HealthStatus.tsx`, donde aporta estado de carga real para dos consultas ejecutadas en el navegador.

## Accesibilidad y responsive

El layout incluye enlace para saltar al contenido, landmarks, jerarquía de encabezados, labels asociados, foco visible, links y botones semánticos, navegación móvil operable con teclado y respeto por `prefers-reduced-motion`. Los grids pasan de tres/cuatro columnas a dos y una; los filtros dejan de ser sticky y se reorganizan para tablet y móvil.

## Pruebas

Vitest cubre normalización/serialización de query params, conservación de filtros al paginar, etiquetas de disponibilidad, ausencia deliberada de URL de media y generación codificada de WhatsApp. `npm run build` ejecuta primero `astro check`.
