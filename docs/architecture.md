# Arquitectura

La arquitectura inicial mantiene tres responsabilidades claramente separadas:

```text
Browser
   ↓
Astro
   ↓ HTTP
ASP.NET Core API
   ↓
PostgreSQL
```

## Frontend

Astro genera las páginas y mantiene JavaScript en el navegador al mínimo. React se usa únicamente para el island interactivo del indicador de salud del backend y la base de datos. El catálogo, sus filtros y la navegación funcionan con HTML y formularios nativos. El cliente HTTP centraliza la URL, los headers y el manejo básico de respuestas; `lib/api/catalog.ts` expone las consultas tipadas del dominio.

La salida usa el adaptador de Node en modo `server`, con renderizado selectivo. Inicio, catálogo, productos, marcas y categorías se renderizan bajo demanda para consultar datos actualizados y entregar HTML inicial útil. Empresas, nosotros, contacto, preguntas frecuentes, health y 404 declaran `prerender = true` y se generan durante el build. El despliegue deberá ejecutar `dist/server/entry.mjs`; no es un sitio puramente estático.

Los filtros del catálogo usan un formulario `GET`. La URL es la fuente de estado y se transmite directamente a la API después de normalizar los parámetros soportados. No existe estado duplicado en React ni una búsqueda implementada en el navegador.

## Backend

Un solo proyecto ASP.NET Core expone endpoints REST mediante controladores. `GET /api/health` comprueba únicamente que la API está viva; `GET /api/ready` usa `AppDbContext` para comprobar que EF Core puede conectarse a PostgreSQL. `AppDbContext` configura EF Core con Npgsql. La estructura se ampliará con servicios, repositorios u otras carpetas solo cuando aparezca lógica que lo necesite.

La API pública del catálogo separa controllers pequeños, servicios de consulta y DTOs públicos. Todas las lecturas usan `AsNoTracking` y proyección LINQ; las entidades de EF Core nunca forman parte del contrato HTTP. La lista de productos ejecuta una consulta de conteo y otra paginada. El detalle utiliza una consulta del producto y dos consultas auxiliares acotadas para imágenes y especificaciones, evitando `Include` amplios y N+1.

El manejador global de excepciones produce un sobre de error estable sin exponer stack traces. CORS acepta únicamente orígenes configurados. El logging usa los proveedores estándar de ASP.NET Core y no debe recibir secretos.

## Datos

PostgreSQL corre en Docker durante el desarrollo. La API corre en el host y recibe la cadena de conexión mediante `ConnectionStrings__Default`. EF Core mantiene el modelo relacional en un único `AppDbContext`; cada entidad tiene una configuración `IEntityTypeConfiguration<T>` explícita y las migraciones viven en `backend/Migrations`.

El modelo inicial cubre catálogo, clientes internos, solicitudes de cotización y material especial, usuarios administrativos sin autenticación y configuración básica del sitio. Los enums se almacenan como enteros y los timestamps persistidos usan `timestamp with time zone` con valores UTC. Las decisiones detalladas de relaciones e integridad referencial están en [database.md](database.md).

## Evolución prevista

Más adelante se añadirá almacenamiento compatible con S3 detrás de una abstracción de storage. No se configura proveedor, bucket ni credenciales en esta fase. Nginx se incorporará al preparar el despliegue en el servidor propio, no durante este bootstrap.

Mientras no exista storage público, `resolveMediaUrl(storageKey)` devuelve `null` y los componentes presentan un fallback visual. Nunca se convierte un `StorageKey` en una URL inventada.
