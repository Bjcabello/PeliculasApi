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
- .NET 9
- AutoMapper version 13.0.1
- Microsoft.EntityFrameworkCore.SqlServer version: 9.0.10
- Microsoft.EntityFrameworkCore.Tools version: 9.0.10
- Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite version: 9.0.10
- Microsoft.AspNetCore.OpenApi version: 9.0.9
- Microsoft.AspNetCore.Identity.EntityFrameworkCore  version: 9.0.10
- Microsoft.AspNetCore.Authentication.JwtBearer version: 9.0.10
- Swashbuckle.AspNetCore version: 9.0.6

## Contribuir
¡Pull requests bienvenidos!