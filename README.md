# Elefante Wines API 

API REST de ecommerce para **Elefante Wines** desarrollada como proyecto de la materia **Backend (Optativa II)** — Tecnicatura Universitaria en Desarrollo de Software, Universidad Católica de Cuyo (San Juan, 2026).

## Arquitectura

El proyecto sigue **Clean Architecture** con 4 capas concéntricas, respetando la **Dependency Rule**: las dependencias apuntan hacia el centro (Domain).

ElefanteWines.Api              → Controllers, Swagger, Program.cs
↓ depende de
ElefanteWines.Infrastructure   → EF Core, DbContext, Repositorios, Configurations
↓ depende de
ElefanteWines.Application      → Interfaces de repositorios (contratos)
↓ depende de
ElefanteWines.Domain           → Entidades + reglas de negocio (núcleo)

**Domain no depende de nada.** Si mañana cambiamos EF Core por MongoDB, solo tocamos Infrastructure.

---

## Stack técnico

- **.NET 8** + ASP.NET Core Web API
- **Entity Framework Core 8** con SQLite
- **Repository Pattern** para abstraer el acceso a datos
- **Dependency Injection** (Scoped) — built-in de .NET
- **Swagger / OpenAPI** para documentación interactiva
- **Migraciones** versionadas + Seed de datos iniciales

---

## Entidades del dominio

- **Wine** — vino con stock, precio, varietal y año
- **Category** — Tintos / Blancos / Espumantes (cargadas por seed)
- **Customer** — cliente con email único
- **Order** — orden de compra con estado (Pending, Confirmed, Shipped, Delivered, Cancelled)
- **OrderItem** — ítem de orden con subtotal calculado

Las reglas de negocio viven en el Domain. Por ejemplo, agregar un ítem a una orden descuenta el stock del vino automáticamente vía `Wine.ReduceStock()`.

---

## Cómo correr el proyecto

### Requisitos
- .NET 8 SDK
- Git

### Pasos

```bash
# 1. Clonar el repo
git clone https://github.com/delfinafraifer/ElefanteWines.git
cd ElefanteWines

# 2. Restaurar paquetes
dotnet restore

# 3. Crear la base de datos (genera el archivo elefantewines.db con tablas + seed)
dotnet ef database update --project ElefanteWines.Infrastructure --startup-project ElefanteWines.Api

# 4. Ejecutar la API
dotnet run --project ElefanteWines.Api

# 5. Abrir Swagger en el navegador
# http://localhost:5239/swagger
```

---

## Endpoints disponibles

### Wines
- `GET /api/wines` — listar todos
- `GET /api/wines/{id}` — obtener uno por id
- `GET /api/wines/search?term=malbec` — buscar por nombre
- `POST /api/wines` — crear vino
- `PUT /api/wines/{id}/price` — actualizar precio
- `DELETE /api/wines/{id}` — eliminar

### Orders
- `GET /api/orders` — listar todas
- `GET /api/orders/{id}` — obtener con sus ítems
- `GET /api/orders/by-customer/{customerId}` — órdenes de un cliente
- `POST /api/orders` — crear orden (descuenta stock automáticamente)

### Customers
- `GET /api/customers` — listar
- `GET /api/customers/{id}` — obtener uno
- `POST /api/customers` — registrar cliente nuevo

### Categories
- `GET /api/categories` — listar (3 categorías cargadas por seed)
- `GET /api/categories/{id}` — obtener una

---

## Decisiones de diseño

- **Guid como id**: generado en el Domain (no autoincremental). Permite crear entidades sin viaje a la BD.
- **`AsNoTracking()` en lecturas**: reduce memoria, EF no trackea entidades que no se van a modificar.
- **Enum como string en BD**: `OrderStatus` se persiste como `"Pending"` en SQLite, no como `0`. Más legible al inspeccionar la BD.
- **Constructores privados sin parámetros**: requeridos por EF Core para materializar entidades desde la BD, pero ocultos para el resto del código.
- **Propiedades con setter privado**: la entidad solo se modifica a través de sus métodos de negocio (`UpdatePrice`, `ReduceStock`, etc.), no por asignación directa.

---

## Unidades cubiertas

- **Unidad 1** — Fundamentos: Clean Architecture, SOLID, Dependency Rule
- **Unidad 2** — .NET Core: Controllers, Dependency Injection, Swagger
- **Unidad 3** — Domain: Entidades, reglas de negocio, encapsulamiento
- **Unidad 3** — Application: Interfaces (puertos)
- **Unidad 4** — Infrastructure: EF Core, DbContext, Fluent API, Repository, Migraciones, Seed

---

## Autora

Proyecto desarrollado por Delfina Fraifer — Tecnicatura Universitaria en Desarrollo de Software, UCCuyo, 2026.