# Manager Money

![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet)
![Docker](https://img.shields.io/badge/Docker-Supported-blue)

Aplicación de gestión de finanzas personales desarrollada con .NET 9, diseñada para ofrecer un seguimiento detallado de ingresos y gastos a través de una interfaz web intuitiva.

## ✨ Características Principales

- **Autenticación Segura:** Registro e inicio de sesión de usuarios.
- **Gestión de Transacciones:** Creación, edición y eliminación de transacciones, asignadas a cuentas y categorías personalizadas.
- **Visualización Interactiva:** Un calendario para ver transacciones por fecha y un desglose detallado de ingresos y gastos.
- **Datos Iniciales:** Creación automática de cuentas y categorías para nuevos usuarios, facilitando el primer uso.
- **Reportes:** Generación de reportes de transacciones.

## 🚀 Tecnologías Utilizadas

- **Backend:** .NET 9, ASP.NET Core Identity
- **Base de Datos:** SQL Server 2019
- **Frontend:** Bootstrap 5, FullCalendar.js, jQuery
- **Contenerización:** Docker & Docker Compose

## 🏁 Empezando

Asegúrate de tener instalados los siguientes requisitos previos:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started) y [Docker Compose](https://docs.docker.com/compose/install/)

### 🐳 Opción 1: Ejecución con Docker (Recomendado)

1.  **Clona el repositorio:**
    ```bash
    git clone https://github.com/cesardev1/ManagerMoney.git
    cd ManagerMoney
    ```

2.  **Configura las variables de entorno:**
    Crea un archivo `.env` en la raíz del proyecto y añade tu contraseña para la base de datos:
    ```dotenv
    SA_PASSWORD=TuContraseñaSegura123!
    ```

3.  **Construye y ejecuta los contenedores:**
    ```bash
    docker-compose up -d
    ```

4.  **Accede a la aplicación:**
    - **Aplicación Web:** `http://localhost:5000`
    - **SQL Server:** `localhost,1433`

### 💻 Opción 2: Ejecución en Local (Sin Docker)

1.  **Configura los secretos de usuario:**
    Inicializa los secretos de .NET en la raíz del proyecto:
    ```bash
    dotnet user-secrets init
    ```

2.  **Establece la cadena de conexión:**
    Añade tu cadena de conexión a SQL Server.
    ```bash
    dotnet user-secrets set "ConnectionString" "Server=localhost;Database=ManagerMoney;Integrated Security=true;TrustServerCertificate=true;"
    ```
    *Nota: Ajusta la cadena de conexión según tu configuración de SQL Server.*

3.  **Ejecuta la aplicación:**
    ```bash
    dotnet run
    ```

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Si deseas mejorar este proyecto, por favor sigue estos pasos:

1.  Haz un Fork del proyecto.
2.  Crea una nueva rama para tu funcionalidad (`git checkout -b feature/AmazingFeature`).
3.  Realiza tus cambios y haz commit (`git commit -m 'Add: alguna característica asombrosa'`).
4.  Sube tus cambios a la rama (`git push origin feature/AmazingFeature`).
5.  Abre un Pull Request.

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Consulta el archivo `LICENSE` para más detalles.

## 📧 Contacto

**César Rodrigo Nieto López** - [cesar.rnl.dev@gmail.com](mailto:cesar.rnl.dev@gmail.com)

Para soporte o preguntas, por favor [abre un issue](https://github.com/cesardev1/ManagerMoney/issues) en el repositorio.