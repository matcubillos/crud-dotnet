# Prueba técnica .Net CRUD
## Herramientas utilizadas
- **Visual Studio 2022** (Community, Professional o Enterprise)
- **.NET 8.0 SDK**
- **MariaDB Server**
  - Versión recomendada: 11.7.2 o superior
  - Descargar desde: https://mariadb.org/download/
  - Configura la conexion con los siguientes parametros:
  - **Port=3306**
  - **Database=clientes**
  - **User=root**
  - **Password=root**

## Cómo Ejecutar la Aplicación

### Opción 1: Visual Studio
1. Abrir el archivo `.sln` con Visual Studio 2022
2. Establecer el proyecto de inicio (si hay múltiples proyectos)
3. Presionar `F5` o hacer clic en "Iniciar depuración"

### Opción 2: Línea de Comandos
```bash
dotnet run
```

### Opción 3: Visual Studio Code
1. Abrir la carpeta del proyecto en VS Code
2. Instalar la extensión "C# Dev Kit"
3. Usar `Ctrl+F5` para ejecutar sin depuración

### URL
#### Swagger: https://localhost:7044/swagger/index.html
#### Api: http://localhost:5296/api/v1/

### Versiones
- Entity Framework Core 8.0.13
- Pomelo.EntityFrameworkCore.MySql 8.0.3 (driver para MariaDB)
- Swashbuckle 6.6.2 (para Swagger/API docs)
