
# Laboratorio #5 - Vue.js + ASP.NET Core

## Descripción
En este laboratorio desarrollé una aplicación fullstack que conecta un frontend en Vue.js con un backend en ASP.NET Core, usando una API REST para gestionar países. Documenté cada paso para que puedas replicar o entender el proceso.

---

## ¿Qué hice? (Guía de desarrollo)

### Backend (.NET Core)
1. Creé la base de datos y la tabla `Country` con las columnas:
    - `Name` (string)
    - `Continent` (string)
    - `Language` (string)
2. Instalé los paquetes NuGet necesarios: Dapper y Microsoft.Data.SqlClient.
3. Configuré CORS en `Program.cs` para aceptar peticiones desde el frontend:
    ```csharp
    builder.Services.AddCors();
    // ...
    app.UseCors(builder =>
         builder.WithOrigins("http://localhost:8080")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
    );
    ```
4. Implementé el repositorio, servicio y controlador para exponer los endpoints de país.
5. Ejecuté el backend con:
    ```bash
    cd backend-lab
    dotnet run
    ```
    El backend quedó corriendo en `https://localhost:7019`.

### Frontend (Vue.js)
1. Inicialicé el proyecto Vue y agregué las dependencias:
    ```bash
    cd frontend-lab
    npm install
    npm install axios
    npm install vue-router@4 --save
    ```
2. Configuré Vue Router con dos rutas: `/` para la lista y `/country` para el formulario.
3. Implementé los componentes:
    - **CountriesList.vue**: muestra la lista de países, permite eliminar y navegar al formulario.
    - **CountryForm.vue**: formulario con validaciones, select de continente, POST a la API y redirección.
4. Arranqué el frontend con:
    ```bash
    npm run serve
    ```
    El frontend quedó corriendo en `http://localhost:8080`.

---

## Estructura del proyecto

```
Laboratorio5_C22306/
├── backend-lab/
│   ├── Controllers/
│   │   └── CountryController.cs
│   ├── Models/
│   │   └── CountryModel.cs
│   ├── Repositories/
│   │   └── CountryRepository.cs
│   ├── Services/
│   │   └── CountryService.cs
│   └── Program.cs
└── frontend-lab/
     ├── src/
     │   ├── components/
     │   │   ├── CountriesList.vue
     │   │   └── CountryForm.vue
     │   └── main.js
     └── ...
```

---

## Validaciones y manejo de errores

- Todos los campos del formulario son requeridos.
- El select de continente tiene opciones predefinidas.
- Si aparece un error de CORS, revisé la configuración en `Program.cs`.
- Si hay error de conexión, verifiqué que el backend esté corriendo y la URL sea correcta.
- Si el navegador advierte sobre el certificado, acepté el riesgo temporalmente para desarrollo.

---

## Modelo de datos esperado

```json
{
  "name": "string",
  "continent": "string",
  "language": "string"
}
```

---

## Guía de uso (¿Cómo lo pruebo?)

### 1. Backend
1. Me aseguro de que la base de datos y la tabla `Country` existen.
2. Ejecuto el backend:
    ```bash
    cd backend-lab
    dotnet run
    ```
3. Pruebo los endpoints con Postman, VS Code REST Client o navegador:
    - **GET países:**
      ```
      GET https://localhost:7019/api/country
      ```
    - **POST país:**
      ```
      POST https://localhost:7019/api/country
      Content-Type: application/json

      {
        "name": "Costa Rica",
        "continent": "América",
        "language": "Español"
      }
      ```

### 2. Frontend
1. Instalo dependencias y arranco el servidor:
    ```bash
    cd frontend-lab
    npm install
    npm run serve
    ```
2. Abro `http://localhost:8080` en el navegador.
3. Pruebo:
    - Ver la lista de países.
    - Agregar un país.
    - Eliminar un país.

---

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