# Desarrollo local

Los comandos siguientes asumen Windows PowerShell y que la terminal está ubicada en la raíz del repositorio.

## 1. Requisitos

- Node.js 24 o posterior y npm 11 o posterior.
- .NET SDK 10.
- Docker Desktop con Docker Compose.
- Puertos locales libres: `4321`, `5000` y `5432`.

Comprueba las herramientas:

```powershell
node --version
npm --version
dotnet --version
docker compose version
```

## 2. Instalar dependencias

```powershell
cd frontend
npm install
cd ..\backend
dotnet restore
dotnet tool restore
cd ..
```

## 3. Configurar variables de entorno

Copia el archivo de ejemplo. `.env` está ignorado por Git y puede contener tus valores locales; no lo subas al repositorio.

```powershell
Copy-Item .env.example .env
```

Edita `.env` y sustituye `replace-with-a-local-password` en `POSTGRES_PASSWORD` y `ConnectionStrings__Default` por la misma contraseña local. No uses una credencial de producción. `PUBLIC_API_URL` contiene una URL pública porque Astro la incorpora al código del navegador; nunca coloques secretos en variables `PUBLIC_`.

Si `5432` ya está ocupado, cambia `POSTGRES_PORT` y el valor `Port` de `ConnectionStrings__Default` al mismo puerto libre, por ejemplo `5433`.

Docker Compose y Astro leen el `.env` de la raíz automáticamente. ASP.NET Core recibe la cadena de conexión desde la terminal que inicia el proceso, como se muestra abajo.

## 4. Levantar PostgreSQL

Desde la raíz:

```powershell
docker compose up -d postgres
docker compose ps
```

Espera hasta que el estado de `postgres` indique `healthy`.

## 5. Arrancar el backend

Aplica primero las migraciones desde `backend`:

```powershell
cd backend
$env:ConnectionStrings__Default = (Get-Content ..\.env | Where-Object { $_ -match '^ConnectionStrings__Default=' }) -replace '^ConnectionStrings__Default=', ''
dotnet tool run dotnet-ef database update
```

Después arranca la API en la misma terminal:

```powershell
dotnet run
```

El perfil de desarrollo configura `http://localhost:5000` y permite CORS desde `http://localhost:4321`. En producción se debe configurar al menos `ConnectionStrings__Default` y `Cors__AllowedOrigins__0` mediante el entorno.

## 6. Arrancar el frontend

Abre otra terminal nueva en la raíz:

```powershell
cd frontend
npm run dev
```

El frontend estará en `http://localhost:4321`.

Astro consulta la API desde el proceso del servidor para las rutas públicas dinámicas. Por eso el backend debe estar disponible al navegar por inicio, catálogo, productos, marcas o categorías. Las páginas institucionales se prerenderizan y no dependen de la API en runtime.

## 7. Probar la API

Comprueba primero el liveness de la API:

```powershell
Invoke-RestMethod http://localhost:5000/api/health
```

Respuesta esperada:

```json
{
  "status": "ok",
  "service": "Control Electrico Obrera API"
}
```

Comprueba después readiness, que valida la conexión de EF Core con PostgreSQL:

```powershell
Invoke-RestMethod http://localhost:5000/api/ready
```

`/api/ready` devuelve `200` con `status: ok` cuando PostgreSQL está disponible y `503` con `status: unavailable` cuando no lo está. `/api/health` continúa devolviendo `200` mientras la API siga viva.

## 8. Probar el frontend

Abre `http://localhost:4321/health`. Debe mostrar:

```text
Frontend  OK
Backend   OK
Database  OK
```

La página consulta `/api/health` y `/api/ready` por separado. Si la API o la base de datos no responden, el estado correspondiente cambia a `No disponible` sin romper la página.

## Comandos de validación

```powershell
cd backend
dotnet build
cd ..
$baseConnection = (Get-Content .env | Where-Object { $_ -match '^ConnectionStrings__Default=' }) -replace '^ConnectionStrings__Default=', ''
$env:TEST_DATABASE_CONNECTION = $baseConnection -replace 'Database=control_electrico_obrera', 'Database=control_electrico_obrera_tests'
dotnet test .\backend.tests\ControlElectricoObrera.Api.Tests.csproj
cd frontend
npm run check
npm test
npm run build
```

El build genera un servidor Node standalone. Para probar exactamente el artefacto compilado:

```powershell
cd frontend
node .\dist\server\entry.mjs
```

En producción, `PUBLIC_API_URL` se define al compilar y su destino debe ser accesible desde el proceso de Astro. Como el island de `/health` también la usa en el navegador, la URL debe seguir siendo pública y CORS debe aceptar el origen del frontend.

## Datos de catálogo para desarrollo

El seed es manual, idempotente, no elimina registros y contiene únicamente productos ficticios. Nunca se ejecuta al arrancar la aplicación.

Desde la raíz:

```powershell
Get-Content .\docker\dev-seed.sql -Raw | docker compose exec -T postgres psql -U control_electrico -d control_electrico_obrera
```

Puede ejecutarse nuevamente: los slugs existentes y los metadatos de imagen/especificación no se duplican.

## Pruebas de integración

Las pruebas usan exclusivamente una base llamada `control_electrico_obrera_tests`. El fixture rechaza cualquier otro nombre, recrea esa base, aplica las migraciones, carga datos ficticios y la elimina al finalizar. No utiliza ni modifica la base de desarrollo.

En PowerShell, desde la raíz:

```powershell
$baseConnection = (Get-Content .env | Where-Object { $_ -match '^ConnectionStrings__Default=' }) -replace '^ConnectionStrings__Default=', ''
$env:TEST_DATABASE_CONNECTION = $baseConnection -replace 'Database=control_electrico_obrera', 'Database=control_electrico_obrera_tests'
dotnet test .\backend.tests\ControlElectricoObrera.Api.Tests.csproj
```

Para comprobar si el modelo cambió sin una migración correspondiente:

```powershell
cd backend
$env:ConnectionStrings__Default = (Get-Content ..\.env | Where-Object { $_ -match '^ConnectionStrings__Default=' }) -replace '^ConnectionStrings__Default=', ''
dotnet tool run dotnet-ef migrations has-pending-model-changes
```

Consulta [database.md](database.md) para crear y aplicar migraciones futuras.

La referencia completa de la API pública está en [api.md](api.md).
La estructura y las decisiones del frontend público están en [frontend.md](frontend.md).

Para detener PostgreSQL:

```powershell
docker compose stop postgres
```

Usa `docker compose down` para retirar el contenedor conservando el volumen nombrado. Agrega `--volumes` únicamente si deseas eliminar también los datos locales.
