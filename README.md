# Manager Money

## 📋 Descripción
Manager Money es una aplicación de gestión de presupuestos personales desarrollada con .NET 9.0, que permite a los usuarios realizar un seguimiento detallado de sus finanzas personales.

## 🚀 Tecnologías
- .NET 9.0
- SQL Server 2019
- Docker
- Docker Compose

## 💻 Requisitos Previos
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (solo para desarrollo)

## 🛠️ Configuración del Entorno Docker

### Variables de Entorno 
Crea un archivo `.env` en la raíz del proyecto con las siguientes variables:

````dotenv
    SA_PASSWORD=TuContraseñaSegura123!
````

### Ejecución 
1. Clona el repositorio:

````bash
git clone [URL-del-repositorio] cd ManagerMoney
````
2. Construye y ejecuta los contenedores:

```bash
docker-compose up -d
```
3. La aplicación estará disponible en:
    - Aplicación Web: [http://localhost:5000](http://localhost:5000)
    - SQL Server: localhost,1433

## 🏗️ Arquitectura
### Contenedores Docker
El proyecto utiliza tres servicios principales:
1. **SQL Server** (Puerto 1433)
    - Base de datos principal
    - Persistencia de datos mediante volúmenes
    - Healthchecks configurados

2. **InitDB**
    - Servicio de inicialización de base de datos
    - Ejecuta scripts de migración inicial
    - Dependiente del servicio SQL Server

3. **ManagerApp** (Puerto 5000)
    - Aplicación principal
    - Interfaz web
    - Conexión automática con la base de datos

## 🔧 Desarrollo Local
### Requisitos para Desarrollo
1. .NET 9.0 SDK
2. IDE (recomendado: Visual Studio 2025 o JetBrains Rider)
3. Docker Desktop

### Comandos Útiles

``` bash
# Construir la aplicación
dotnet build

# Ejecutar pruebas
dotnet test

# Ejecutar la aplicación localmente
dotnet run

# Ver logs de Docker
docker-compose logs -f

# Detener contenedores
docker-compose down
```

## 🛠️ Configuración del Entorno de desarollo sin Docker

Crea el archivo de `secrets` para el manejo de secretos seguros

```bash
dotnet user-secrets init
```
Agregar la cadena de conexion a los secretos con el comando: 
```bash
dotnet user-secrets set "ConnectionString" "<Escribe tu cadena de conexion aqui>"
```
> 
> Si SqlServer se encuentra en un entorno local regularmente tiene el siguiente formato:
>
> Con integracion del usuario windows
> ```dotenv
> "Server=localhost;Database=MiBaseDeDatos;Integrated Security=true;TrustServerCertificate=true;"
>```
> Con Usuario y contraseña de BD:
> ````bash
> "Server=localhost;Database=MiBaseDeDatos;User Id=miusuario;Password=micontraseña;TrustServerCertificate=true;"
> ````
>



## 📚 Base de Datos
### Estructura
La base de datos `ManejoPresupuesto` incluye las siguientes tablas principales:
- Categorias
- (otras tablas relevantes)

### Migraciones
Las migraciones se ejecutan automáticamente al iniciar el contenedor `initdb`.
## 🤝 Contribución
1. Fork el proyecto
2. Crea tu rama de características (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add: alguna característica asombrosa'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## ⚙️ Configuración Avanzada
### Healthchecks
Los servicios incluyen healthchecks para garantizar la disponibilidad:
- SQL Server: Verifica la conexión cada 10 segundos
- Aplicación: Dependencias configuradas con health checks

### Persistencia de Datos
Los datos se mantienen en volúmenes Docker:
- `sql_data`: Almacena los archivos de la base de datos

## 🔐 Seguridad
- Las contraseñas y credenciales se manejan mediante variables de entorno
- TLS/SSL habilitado para conexiones de base de datos
- Certificados del servidor SQL confiables configurados

## 📝 Licencia

## 👥 Autores
- César Rodrigo Nieto López

## 🆘 Soporte
Para soporte, por favor abre un issue en el repositorio.

