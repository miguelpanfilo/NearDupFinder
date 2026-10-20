# NearDupFinder — Detector de productos duplicados

Aplicación web para **detectar y gestionar productos casi duplicados** en catálogos. Compara los ítems por similitud de texto, los agrupa en *clusters* y permite fusionarlos, gestionar stock y exportar los resultados.

> Proyecto obligatorio de **Diseño de Aplicaciones 1** — Universidad ORT Uruguay (Licenciatura en Sistemas).
> Desarrollado en equipo por **Miguel Panfilo**, [@ManuPerez](https://github.com/ManuPerez) y [@marianoapn](https://github.com/marianoapn)

---

##  Funcionalidades

- **Detección de duplicados:** similitud de **Jaccard** sobre título, descripción, marca y modelo (con pesos por campo), con **stemming en español** (Lucene.Net). Clasifica cada par como *posible duplicado* o *alerta de duplicado* según umbrales configurados.
- **Clusters de duplicados:** agrupación de ítems similares y fusión en un ítem canónico.
- **Catálogos e ítems:** alta, edición y listado de catálogos y productos.
- **Importación CSV** de ítems y **exportación a Excel** de resultados.
- **Stock y reservas:** reserva de stock por cluster y notificaciones cuando el stock baja del mínimo.
- **Usuarios y roles:** login con roles *Administrador*, *Revisor* y *Operario*; gestión de usuarios y cambio de contraseña.
- **Auditoría:** registro de las acciones de los usuarios.

##  Stack

| Capa | Tecnologías |
|---|---|
| Frontend | **Blazor** (.NET 8) |
| Backend | **C# / .NET 8**, arquitectura en capas |
| Persistencia | **Entity Framework Core 8** + **SQL Server** (Azure SQL Edge en **Docker**), migraciones |
| Librerías | Lucene.Net (stemming), CsvHelper, ClosedXML |
| Testing | **MSTest** + **Moq**, EF Core InMemory · **440+ tests unitarios** |
| CI | **GitHub Actions** (build + test en cada push / PR) |

##  Arquitectura

```
NearDupFinder_Interfaz        → UI en Blazor (páginas y componentes)
NearDupFInder_LogicaDeNegocio → Servicios: duplicados, clusters, reservas, notificaciones, usuarios
NearDupFinder_Dominio         → Entidades del dominio (Item, Catalogo, Cluster, Usuario, etc)
NearDupFinder_Interfaces      → Contratos / interfaces de repositorios y servicios
NearDupFinder_Almacenamiento  → Acceso a datos con EF Core (SqlContext, repositorios, migraciones)
NearDupFinder_Pruebas         → Tests unitarios
```

Desarrollado con **TDD** y **Clean Code**, separando responsabilidades en capas con interfaces e inyección de dependencias.

##  Cómo ejecutarlo

Requisitos: [.NET 8 SDK](https://dotnet.microsoft.com/download) y [Docker](https://www.docker.com/).

```bash
# 1. Levantar SQL Server en Docker
docker compose up -d

# 2. Ejecutar la aplicación (las migraciones se aplican al iniciar)
dotnet run --project NearDupFinder_Interfaz
```

Abrir `http://localhost:5040`.

### Tests

```bash
dotnet test
```

## 📸 Capturas

<!-- Agregá acá 2 o 3 capturas: lista de duplicados, clusters e importación CSV -->
<!-- ![Duplicados](docs/duplicados.png) -->

---

**Miguel Panfilo** — [LinkedIn](https://www.linkedin.com/in/miguel-panfilo-214aa2127) · [GitHub](https://github.com/miguelpanfilo)
