# Registro de cambios

Este archivo distingue cambios completados de trabajo pendiente. No hay un lanzamiento v1.0.0 publicado.

## Versión 1 — En desarrollo

### 2026-10-02 — Rama y documentación

- Creada la rama version-1 desde main, en el commit de reorganización 78b308e.
- Añadido README.md como entrada al proyecto.
- Añadido docs/PROYECTO.md con arquitectura, estructura real, configuración, modelo SQL previsto, hallazgos y criterios de aceptación.
- Añadido este registro de cambios.
- Documentados los pendientes de mapeo 307, recuperación, SQL, logs y validación en la tienda.
- Esta entrega modifica documentación; no corrige aún el flujo de impresión.

### 2026-10-02 — Reorganización de la solución

Commit: [78b308e](https://github.com/fabriyvera/PrintProfit.Worker/commit/78b308ed369a0f403af311e1170bfac24f77191e).

- Incorporado PrintProfit.Domain, actualmente con Class1 como clase vacía.
- Movidos los archivos del Worker a PrintProfit.Worker/.
- Movida la solución a la raíz y actualizadas sus rutas.
- Conservada la referencia relativa entre Worker y Domain.
- Ampliado .gitignore para excluir archivos generados y configuración local.
- Conservados el historial y el remoto de GitHub.

## Base anterior a la reorganización

- Worker Service dirigido a net9.0.
- Registro como servicio Windows mediante AddWindowsService.
- Opciones del canal de impresión mediante PrintServiceOptions.
- Escucha de eventos 307 mediante EventLogWatcher.
- DTO de inserción y repositorio con llamada a dbo.usp_InsertPrintJobWithCharge.
- Paquetes de Serilog declarados, sin integración configurada en Program.cs.

Estos componentes se revisaron estáticamente; su existencia no certifica compilación ni funcionamiento de extremo a extremo.

## Próximas entregas

1. Verificar compilación y corregir el mapeo del evento 307.
2. Versionar la base SQL y probar el contrato de persistencia y cobro.
3. Implementar recuperación de eventos y manejo de fallos temporales.
4. Configurar logs y validar servicio Windows, colas y permisos.
5. Crear y conciliar las vistas KPI y el reporte Power BI.

Las próximas entregas son plan de trabajo, no cambios implementados.
