# Laboratorio: API .NET 8 + SQL Server en Linux

## Pasos realizados

### 1. Instalación de .NET 8 en Linux

```sh
sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
```

Agrega .NET al PATH (opcional):

```sh
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools' >> ~/.bashrc
source ~/.bashrc
```

---

### 2. Crear el proyecto Web API

```sh
dotnet new webapi -n backend-lab-C22306
cd backend-lab-C22306
```

---

### 3. Ejecutar la API

```sh
dotnet run
```

La API estará disponible en:  
`http://localhost:5172/swagger/index.html` (Swagger UI)  
`http://localhost:5172/api/country` (endpoint de ejemplo)

---

### 4. Agregar el controlador CountryController

Crea el archivo `Controllers/CountryController.cs` con el siguiente contenido:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace backend_lab_C22306.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hola Mundo";
        }
    }
}
```

---

### 5. Instalar y configurar SQL Server en Docker

Descarga la imagen oficial:

```sh
sudo docker pull mcr.microsoft.com/mssql/server:2022-latest
```

Crea y ejecuta el contenedor:

```sh
sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MyStrongPassword123" \
-p 1433:1433 --name sql-server-linux -d mcr.microsoft.com/mssql/server:2022-latest
```

Para persistencia de datos (ejemplo con versión 2025):

```sh
sudo docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=MyStrongPassword123" \
  -p 1433:1433 \
  --name sqlserver2025 \
  -v ~/sqlserver_data_2025:/var/opt/mssql \
  -d mcr.microsoft.com/mssql/server:2025-latest
```

---

### 6. Instalar herramientas de SQL Server en Linux

```sh
curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
sudo add-apt-repository "$(wget -qO- https://packages.microsoft.com/config/ubuntu/20.04/prod.list)"
sudo apt update
sudo apt install -y mssql-tools unixodbc-dev
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc
source ~/.bashrc
```

Verifica la instalación:

```sh
sqlcmd -?
```

Conéctate al servidor SQL:

```sh
sqlcmd -S localhost,1433 -U SA -P "MyStrongPassword123"
```

---

### 7. Instalar paquetes NuGet necesarios

```sh
dotnet add package Dapper
dotnet add package Microsoft.Data.SqlClient
```

---

### 8. Probar la API

- Accede a `http://localhost:5172/api/country` para ver el mensaje `"Hola Mundo"`.
- Accede a `http://localhost:5172/swagger/index.html` para la documentación interactiva.

---

## Notas

- El template de Web API en .NET 8 usa minimal APIs en `Program.cs` y no incluye la carpeta `Controllers` por defecto.
- Puedes agregar tus propios controladores y modelos según lo necesites.
- Recuerda configurar la cadena de conexión en `appsettings.json` para conectar tu API con SQL Server.

---

**¡Listo! Tu API .NET 8 está corriendo en Linux y conectada a SQL