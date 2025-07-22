# Arquitectura del Sistema

Este documento describe la arquitectura técnica, la estructura de la base de datos y otros detalles de implementación del proyecto Manager Money.

## 🏗️ Arquitectura de Contenedores

El `docker-compose.yaml` define tres servicios principales:

1.  **sql-server:**
    - **Imagen:** `mcr.microsoft.com/mssql/server:2019-latest`
    - **Puerto:** `1433:1433`
    - **Descripción:** Es la base de datos principal que almacena todos los datos de la aplicación. Utiliza un volumen (`sql_data`) para garantizar la persistencia de los datos.

2.  **init-db:**
    - **Imagen:** Construida desde `initdb/Dockerfile`
    - **Descripción:** Un contenedor de corta duración que espera a que `sql-server` esté disponible y luego ejecuta el script `init.sql` para crear la base de datos y las tablas iniciales.

3.  **manager-app:**
    - **Imagen:** Construida desde el `Dockerfile` principal.
    - **Puerto:** `5000:8080`
    - **Descripción:** La aplicación web principal de .NET. Depende de que `init-db` haya completado su trabajo.

## 📚 Base de Datos

- **Sistema Gestor:** SQL Server 2019
- **Nombre de la BD:** `ManejoPresupuesto`

### Tablas Principales
-   **Users**: Gestión de usuarios (integrada con ASP.NET Core Identity).
-   **Categorias**: Almacena las categorías de ingresos y gastos.
-   **TiposCuentas**: Define los tipos de cuentas (Ej: Efectivo, Banco, Tarjetas).
-   **Cuentas**: Contiene las cuentas específicas de cada usuario.
-   **Transacciones**: Registra cada movimiento financiero.

### Scripts de Inicialización
El script `sql/init.sql` se encarga de:
- Crear la base de datos `ManejoPresupuesto` si no existe.
- Crear las tablas principales.
- Definir los procedimientos almacenados necesarios para la lógica de negocio inicial.

## 🔐 Seguridad

- **Gestión de Secretos:** Las credenciales de la base de datos se gestionan a través de variables de entorno (`.env` con Docker) o `user-secrets` en desarrollo local.
- **Autorización:** La aplicación implementa una política de autorización global, requiriendo que los usuarios estén autenticados para acceder a la mayoría de los endpoints.
