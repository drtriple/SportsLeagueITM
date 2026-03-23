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

### Archivos Actualizados / Nuevos
```
├── SportsLeague.sln
├── SportsLeague.API/
│ ├── Controllers/
│ │ └── PlayerController.cs ← NUEVO
│ ├── DTOs/
│ │ ├── Request/
│ │ │ └── PlayerRequestDTO.cs ← NUEVO
│ │ └── Response/
│ │ └── PlayerResponseDTO.cs ← NUEVO
│ ├── Mappings/
│ │ └── MappingProfile.cs (actualizado)
│ ├── Middlewares/
│ ├── Program.cs (actualizado)
│ └── appsettings.json
├── SportsLeague.Domain/
│ ├── Entities/
│ │ ├── Team.cs (actualizado)
│ │ └── Player.cs ← NUEVO
│ ├── Enums/
│ │ └── PlayerPosition.cs ← NUEVO
│ ├── Interfaces/
│ │ ├── Repositories/
│ │ │ └── IPlayerRepository.cs ← NUEVO
│ │ └── Services/
│ │ └── IPlayerService.cs ← NUEVO
│ └── Services/
│ └── PlayerService.cs ← NUEVO
└── SportsLeague.DataAccess/
├── Context/
│ └── LeagueDbContext.cs (actualizado)
├── Repositories/
│ └── PlayerRepository.cs ← NUEVO
```

## Fase 3

### Migraciones Aplicada
```
dotnet ef migrations add AddReferee_Tournament_TournamentTeam --project SportsLeague.DataAccess --startup-project SportsLeague.API

dotnet ef database update --project SportsLeague.DataAccess --startup-project SportsLeague.API

info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (9ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT 1
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (6ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT OBJECT_ID(N'[__EFMigrationsHistory]');
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT 1
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT OBJECT_ID(N'[__EFMigrationsHistory]');
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [MigrationId], [ProductVersion]
      FROM [__EFMigrationsHistory]
      ORDER BY [MigrationId];
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20260322232248_AddReferee_Tournament_TournamentTeam'.
Applying migration '20260322232248_AddReferee_Tournament_TournamentTeam'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (14ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [Referees] (
          [Id] int NOT NULL IDENTITY,
          [FirstName] nvarchar(80) NOT NULL,
          [LastName] nvarchar(80) NOT NULL,
          [Nationality] nvarchar(80) NOT NULL,
          [CreatedAt] datetime2 NOT NULL,
          [UpdatedAt] datetime2 NULL,
          CONSTRAINT [PK_Referees] PRIMARY KEY ([Id])
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [Tournaments] (
          [Id] int NOT NULL IDENTITY,
          [Name] nvarchar(150) NOT NULL,
          [Season] nvarchar(20) NOT NULL,
          [StartDate] datetime2 NOT NULL,
          [EndDate] datetime2 NOT NULL,
          [Status] int NOT NULL,
          [CreatedAt] datetime2 NOT NULL,
          [UpdatedAt] datetime2 NULL,
          CONSTRAINT [PK_Tournaments] PRIMARY KEY ([Id])
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [TournamentTeams] (
          [Id] int NOT NULL IDENTITY,
          [TournamentId] int NOT NULL,
          [TeamId] int NOT NULL,
          [RegisteredAt] datetime2 NOT NULL,
          [CreatedAt] datetime2 NOT NULL,
          [UpdatedAt] datetime2 NULL,
          CONSTRAINT [PK_TournamentTeams] PRIMARY KEY ([Id]),
          CONSTRAINT [FK_TournamentTeams_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([Id]) ON DELETE CASCADE,
          CONSTRAINT [FK_TournamentTeams_Tournaments_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments] ([Id]) ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_TournamentTeams_TeamId] ON [TournamentTeams] ([TeamId]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE UNIQUE INDEX [IX_TournamentTeams_TournamentId_TeamId] ON [TournamentTeams] ([TournamentId], [TeamId]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
      VALUES (N'20260322232248_AddReferee_Tournament_TournamentTeam', N'8.0.25');
Done.
```

### Archivos Nuevos / actualizados
```
SportsLeague.API/Controllers/RefereeController.cs
SportsLeague.API/Controllers/TournamentController.cs
SportsLeague.API/DTOs/Request/RefereeRequestDTO.cs
SportsLeague.API/DTOs/Request/RegisterTeamDTO.cs
SportsLeague.API/DTOs/Request/TournamentRequestDTO.cs
SportsLeague.API/DTOs/Request/UpdateStatusDTO.cs
SportsLeague.API/DTOs/Response/RefereeResponseDTO.cs
SportsLeague.API/DTOs/Response/TournamentResponseDTO.cs
SportsLeague.DataAccess/Repositories/RefereeRepository.cs
SportsLeague.DataAccess/Repositories/TournamentRepository.cs
SportsLeague.DataAccess/Repositories/TournamentTeamRepository.cs
SportsLeague.Domain/Entities/Referee.cs
SportsLeague.Domain/Entities/Tournament.cs
SportsLeague.Domain/Entities/TournamentTeam.cs
SportsLeague.Domain/Enums/TournamentStatus.cs
SportsLeague.Domain/Interfaces/Repositories/IRefereeRepository.cs
SportsLeague.Domain/Interfaces/Repositories/ITournamentRepository.cs
SportsLeague.Domain/Interfaces/Repositories/ITournamentTeamRepository.cs
SportsLeague.Domain/Interfaces/Services/IRefereeService.cs
SportsLeague.Domain/Interfaces/Services/ITournamentService.cs
SportsLeague.Domain/Services/RefereeService.cs
SportsLeague.Domain/Services/TournamentService.cs
```