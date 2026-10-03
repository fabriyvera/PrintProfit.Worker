# PrintProfit

Sistema local para registrar trabajos de impresión, calcular cargos por páginas B/N y color, y alimentar reportes en Power BI mediante SQL Server Express y un Worker Service de .NET.

**Estado:** versión 1 en desarrollo. La rama `version-1` parte de la solución reorganizada; todavía no constituye una versión lista para producción.

## Documentación

- [Documentación del proyecto y plan de versión 1](docs/PROYECTO.md)
- [Registro de cambios](CHANGELOG.md)

## Estructura

```text
PrintProfit/
├── .gitignore
├── README.md
├── CHANGELOG.md
├── PrintProfit.Worker.sln
├── PrintProfit.Domain/
│   ├── Class1.cs
│   └── PrintProfit.Domain.csproj
├── PrintProfit.Worker/
│   ├── Program.cs
│   ├── PrintIngestionService.cs
│   ├── PrintOperationalLogReader.cs
│   ├── SqlPrintRepository.cs
│   ├── PrintJobInsertDto.cs
│   ├── PrintServiceOptions.cs
│   ├── Worker.cs
│   ├── appsettings.json
│   ├── PrintProfit.Worker.csproj
│   └── Properties/launchSettings.json
└── docs/
    └── PROYECTO.md
```

La carpeta local se llama PrintProfit; el repositorio de GitHub conserva el nombre PrintProfit.Worker.

## Desarrollo

Desde una copia local actualizada, sin una rama local version-1 todavía:

```bash
git fetch origin
git switch --track origin/version-1
```

Si la rama local ya existe:

```bash
git switch version-1
git pull --ff-only
```

El Worker apunta a `net9.0-windows`; Domain mantiene `net9.0`. Desde la raíz de la solución, con el SDK correspondiente instalado:

```powershell
dotnet restore PrintProfit.Worker.sln
dotnet build PrintProfit.Worker.sln -c Release
```

La ejecución del lector de eventos requiere Windows. WSL puede usarse para Git, pero no sustituye el acceso al canal de eventos de Windows.

**Antes de ejecutar el Worker:** corregir el mapeo del evento 307 y disponer de la base de datos y el procedimiento SQL compatibles. Sus scripts aún no están incluidos en el repositorio. La documentación explica las dependencias y las validaciones pendientes.
