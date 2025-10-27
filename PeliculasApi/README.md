# PeliculasApi

API backend para gestión de películas usando ASP.NET Core, Entity Framework y SQL Server.

## Características
- CRUD de géneros de películas.
- Paginación y filtros.
- CORS configurado para Angular (localhost:4200).
- AutoMapper para DTOs.

## Instalación
1. Clona el repo: `git clone https://github.com/tuusuario/PeliculasApi.git`
2. Restaura paquetes: Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools, AutoMapper version 13.0.1, Swashbuckle.AspNetCore
3. Aplica migraciones: `Add-Migration TablaGenero` y `Update-Database`
4. Ejecuta: ` run`

## Uso
- Swagger: `https://localhost:7222/swagger`
- Endpoint ejemplo: GET `/api/generos?pagina=1&recordsPorPagina=5`

## Tecnologías
- .NET 8
- Entity Framework Core
- AutoMapper
- Swagger/OpenAPI

## Contribuir
¡Pull requests bienvenidos!