# Control Eléctrico Obrera

Plataforma web para un negocio de material eléctrico industrial en la colonia Obrera, Ciudad de México. Esta etapa incluye el catálogo público de solo lectura, navegación por marcas y categorías, páginas de producto, contacto por WhatsApp y la base de datos para futuras cotizaciones y administración. Todavía no implementa autenticación, panel administrativo, cotizaciones persistidas ni comercio electrónico.

## Stack

- Frontend: Astro 7, TypeScript estricto y React para islands interactivas.
- Backend: ASP.NET Core 10, C# y REST API.
- Datos: Entity Framework Core 10, Npgsql y PostgreSQL 18.
- Desarrollo local: PostgreSQL en Docker Compose; frontend y backend fuera de Docker.

## Estructura

```text
control-electrico-obrera/
├── frontend/           # Aplicación Astro
├── backend/            # API ASP.NET Core
├── backend.tests/      # Pruebas de integración de la API
├── docs/               # Definición, arquitectura y guía de desarrollo
├── docker/             # Notas de infraestructura local
├── .env.example
├── .gitignore
├── docker-compose.yml
└── README.md
```

## Quick start

Requisitos: Node.js 24 o posterior, npm 11 o posterior, .NET SDK 10 y Docker Desktop con Compose.

```powershell
Copy-Item .env.example .env
docker compose up -d postgres
```

En una terminal PowerShell:

```powershell
cd backend
$env:ConnectionStrings__Default = (Get-Content ..\.env | Where-Object { $_ -match '^ConnectionStrings__Default=' }) -replace '^ConnectionStrings__Default=', ''
dotnet restore
dotnet run
```

En otra terminal PowerShell:

```powershell
cd frontend
npm install
npm run dev
```

Visita `http://localhost:4321` y `http://localhost:4321/health`. La API expone liveness en `http://localhost:5000/api/health` y readiness de PostgreSQL en `http://localhost:5000/api/ready`.

## Documentación

- [Definición del proyecto](docs/project-definition.md)
- [Arquitectura](docs/architecture.md)
- [API pública](docs/api.md)
- [Frontend público](docs/frontend.md)
- [Desarrollo local](docs/development.md)
