# SportsLeagueITM

Proyecto Academico.

#### Flujo de una petición HTTP
```
Request HTTP
↓
Controller (API) → Recibe DTO, mapea a Entity, llama al Service
↓
Service (Domain) → Ejecuta lógica de negocio, llama al Repository
↓
Repository (DataAccess) → Ejecuta queries con EF Core contra la BD
↓
SQL Server
```

#### Construcción base

```
# Proyecto API (Web API)
dotnet new webapi -n SportsLeague.API -controllers

# Proyecto Domain (Class Library)
dotnet new classlib -n SportsLeague.Domain

# Proyecto DataAccess (Class Library)
dotnet new classlib -n SportsLeague.DataAccess

dotnet sln add SportsLeague.API/SportsLeague.API.csproj
dotnet sln add SportsLeague.Domain/SportsLeague.Domain.csproj
dotnet sln add SportsLeague.DataAccess/SportsLeague.DataAccess.csproj

# API referencia a Domain
dotnet add SportsLeague.API/SportsLeague.API.csproj reference SportsLeague.Domain/SportsLeague.Domain.csproj


# API referencia a DataAccess (para registrar servicios en Program.cs)
dotnet add SportsLeague.API/SportsLeague.API.csproj reference SportsLeague.DataAccess/SportsLeague.DataAccess.csproj


# DataAccess referencia a Domain
dotnet add SportsLeague.DataAccess/SportsLeague.DataAccess.csproj reference SportsLeague.Domain/SportsLeague.Domain.csproj
```

#### Paqueteria

```
cd SportsLeague.DataAccess
dotnet add package Microsoft.EntityFrameworkCore -v 8.0.*
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 8.0.*
dotnet add package Microsoft.EntityFrameworkCore.Tools -v 8.0.*

cd SportsLeague.API
dotnet add package Microsoft.EntityFrameworkCore.Design -v 8.0.*
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package Swashbuckle.AspNetCore

cd SportsLeague.Domain
dotnet add package Microsoft.Extensions.Logging.Abstractions
```

#### Herramientas
1. Visual Studio 2022
2. Net 9.0
3. SQL Server

#### Iniciar API

```
cd SportsLeague.API
dotnet run
```

## Fase 1

### Migraciones Aplicada
```
dotnet ef migrations add InitialCreate --project SportsLeague.DataAccess --startup-project SportsLeague.API

dotnet ef database update --project SportsLeague.DataAccess --startup-project SportsLeague.API
```

### Estructura establecida
```
SportsLeague/
├── SportsLeague.sln
├── SportsLeague.API/
│ ├── Controllers/
│ │ └── TeamController.cs
│ ├── DTOs/
│ │ ├── Request/
│ │ │ └── TeamRequestDTO.cs
│ │ └── Response/
│ │ └── TeamResponseDTO.cs
│ ├── Mappings/
│ │ └── MappingProfile.cs
│ ├── Middlewares/
│ ├── Program.cs
│ └── appsettings.json
├── SportsLeague.Domain/
│ ├── Entities/
│ │ ├── AuditBase.cs ← NUEVA
│ │ └── Team.cs
│ ├── Enums/
│ ├── Interfaces/
│ │ ├── Repositories/
│ │ │ ├── IGenericRepository.cs
│ │ │ └── ITeamRepository.cs
│ │ └── Services/
│ │ └── ITeamService.cs
│ └── Services/
│ └── TeamService.cs
└── SportsLeague.DataAccess/
├── Context/
│ └── LeagueDbContext.cs
├── Repositories/
│ ├── GenericRepository.cs
│ └── TeamRepository.cs
└── Migrations/
```

### Errores y Soluciones Identificadas

```
Error: System.Reflection.ReflectionTypeLoadException
Could not load type 'Microsoft.OpenApi.Any.IOpenApiAny'
from assembly 'Microsoft.OpenApi, Version=2.4.1.0'

Solución:
SportsLeague.API.SportsLeague.API.csproj
Cambiar la versión de OpenApi (esto depende de la versión que se este manejando):
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.13" />
↓
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.*" />
```

## Fase 2

### Migraciones Aplicada
```
dotnet ef migrations add AddPlayerEntity --project SportsLeague.DataAccess --startup-project SportsLeague.API

dotnet ef database update --project SportsLeague.DataAccess --startup-project SportsLeague.API

dotnet ef migrations add UpdateSlnErrorPlayerEntity --project SportsLeague.DataAccess --startup-project SportsLeague.API

dotnet ef database update --project SportsLeague.DataAccess --startup-project SportsLeague.API
```

### Estructura establecida
```
├── SportsLeague.sln
├── SportsLeague.API/
│ ├── Controllers/
│ │ ├── TeamController.cs
│ │ └── PlayerController.cs ← NUEVO
│ ├── DTOs/
│ │ ├── Request/
│ │ │ ├── TeamRequestDTO.cs
│ │ │ └── PlayerRequestDTO.cs ← NUEVO
│ │ └── Response/
│ │ ├── TeamResponseDTO.cs
│ │ └── PlayerResponseDTO.cs ← NUEVO
│ ├── Mappings/
│ │ └── MappingProfile.cs (actualizado)
│ ├── Middlewares/
│ ├── Program.cs (actualizado)
│ └── appsettings.json
├── SportsLeague.Domain/
│ ├── Entities/
│ │ ├── AuditBase.cs
│ │ ├── Team.cs (actualizado)
│ │ └── Player.cs ← NUEVO
│ ├── Enums/
│ │ └── PlayerPosition.cs ← NUEVO
│ ├── Interfaces/
│ │ ├── Repositories/
│ │ │ ├── IGenericRepository.cs
│ │ │ ├── ITeamRepository.cs
│ │ │ └── IPlayerRepository.cs ← NUEVO
│ │ └── Services/
│ │ ├── ITeamService.cs
│ │ └── IPlayerService.cs ← NUEVO
│ └── Services/
│ ├── TeamService.cs
│ └── PlayerService.cs ← NUEVO
└── SportsLeague.DataAccess/
├── Context/
│ └── LeagueDbContext.cs (actualizado)
├── Repositories/
│ ├── GenericRepository.cs
│ ├── TeamRepository.cs
│ └── PlayerRepository.cs ← NUEVO
```