# Elefante Wines API

API REST de e-commerce para Elefante Wines, una bodega de San Juan. Proyecto final de la materia Backend (Optativa II) - Tecnicatura Universitaria en Desarrollo de Software, UCCuyo, 2026.

## Arquitectura

Clean Architecture con 4 capas. Las dependencias apuntan hacia adentro (Domain es el núcleo y no depende de nada).
ElefanteWines.Api              -> Controllers, Swagger, Program.cs
ElefanteWines.Infrastructure   -> EF Core, DbContext, Repositorios
ElefanteWines.Application      -> Casos de uso (Commands/Queries) e interfaces
ElefanteWines.Domain           -> Entidades y reglas de negocio

## Stack

- .NET 8 + ASP.NET Core Web API
- Entity Framework Core 8 con SQLite
- MediatR para implementar CQRS
- Repository Pattern
- Swagger / OpenAPI
- Migraciones y seed de datos

## Casos de uso

La capa Application implementa los casos de uso separando Commands (escritura) y Queries (lectura).

Commands:
- CreateWineCommand - crea un vino
- CreateOrderCommand - crea una orden, valida cliente y descuenta stock de cada vino

Queries:
- GetAllWinesQuery - lista todos los vinos
- GetWineByIdQuery - obtiene un vino por id

El flujo de un request es: Controller recibe el HTTP, lo convierte en Command/Query, lo manda al Handler vía MediatR, el Handler usa el repositorio para persistir o leer, y el repositorio habla con EF Core. El controller no tiene lógica de negocio.

## Entidades

- Wine: vino con stock, precio, varietal y año
- Category: Tintos, Blancos y Espumantes (cargadas por seed)
- Customer: cliente, con email único
- Order: orden de compra con estado (Pending, Confirmed, Shipped, Delivered, Cancelled)
- OrderItem: ítem de una orden, subtotal calculado

Las reglas de negocio viven en el Domain. Por ejemplo, cuando se agrega un ítem a una orden, la entidad Order llama internamente a Wine.ReduceStock() para descontar stock.

## Cómo correrlo

Requiere .NET 8 SDK y Git.

```bash
git clone https://github.com/delfinafraifer/elefante-wines-api.git
cd elefante-wines-api

dotnet restore

dotnet ef database update --project ElefanteWines.Infrastructure --startup-project ElefanteWines.Api

dotnet run --project ElefanteWines.Api
```

Después abrir en el navegador: http://localhost:5239/swagger

## Endpoints

Wines:
- GET /api/wines
- GET /api/wines/{id}
- GET /api/wines/search?term=malbec
- POST /api/wines
- PUT /api/wines/{id}/price
- DELETE /api/wines/{id}

Orders:
- GET /api/orders
- GET /api/orders/{id}
- GET /api/orders/by-customer/{customerId}
- POST /api/orders

Customers:
- GET /api/customers
- GET /api/customers/{id}
- POST /api/customers

Categories:
- GET /api/categories
- GET /api/categories/{id}

## Notas de implementación

- Los ids son Guid generados en el Domain, no autoincrementales.
- Uso AsNoTracking() en las queries de solo lectura.
- El enum OrderStatus se guarda como string en SQLite y se serializa como string en el JSON.
- Las entidades tienen constructor privado sin parámetros (lo necesita EF Core) y los setters son privados, así se modifica solo a través de métodos de negocio.
- CQRS se aplica en los casos de uso principales (crear vino, crear orden, listar/obtener vinos). En operaciones simples como DELETE o actualizar precio uso Repository directo porque agregar un Command no aportaba mucho.

## Unidades

- Unidad 1: Clean Architecture, SOLID, Dependency Rule
- Unidad 2: .NET Core, Controllers, DI, Swagger
- Unidad 3 Domain: Entidades, reglas de negocio
- Unidad 3 Application: Commands, Queries, MediatR, interfaces de repositorios
- Unidad 4 Infrastructure: EF Core, Fluent API, Repository, Migraciones, Seed

## Autora

Delfina Fraifer