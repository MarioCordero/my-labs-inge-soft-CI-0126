# Laboratorio #5 - Vue.js + ASP.NET Core

## Descripción
En este laboratorio desarrollé una aplicación fullstack que conecta un frontend en Vue.js con un backend en ASP.NET Core, usando una API REST para gestionar países. Toda la consulta y administración de la base de datos la realicé directamente desde Visual Studio Code, usando extensiones para conectarme y ejecutar queries en SQL Server.

---

## ¿Qué hice? (Guía de desarrollo)

### Backend (.NET Core)

**Conexión y consulta a la base de datos desde VS Code:**
- Utilicé la extensión **SQL Server (mssql)** de VS Code para conectarme a la base de datos SQL Server que corre en Docker.
- Desde la barra lateral de VS Code, abrí la extensión, configuré la conexión (localhost, puerto 1433, usuario SA, contraseña) y ejecuté queries directamente sobre la base de datos y la tabla `Country`.

**Comandos útiles para la base de datos:**
- Para acceder al contenedor (opcional):
    ```sh
    sudo docker exec -it sqlserver2025 /bin/bash
    ```
- Para crear y correr SQL Server en Docker:
    ```sh
    sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MyStrongPassword123" \
    -p 1433:1433 --name sqlserver2025 -d mcr.microsoft.com/mssql/server:2022-latest
    ```

**Pasos principales:**
1. Creé la base de datos y la tabla `Country` desde VS Code usando la extensión SQL Server.
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

---

### Frontend (Vue.js)
1. Inicialicé el proyecto Vue y agregué las dependencias:
    ```bash
    cd frontend-lab
    npm install
    npm install axios
    npm install vue-router@4 --save
    ```
2. **Configuré Vue Router** para la navegación entre componentes:
    - En `src/main.js` importé y configuré las rutas:
      ```js
      import { createRouter, createWebHistory } from 'vue-router'
      import CountriesList from './components/CountriesList.vue'
      import CountryForm from './components/CountryForm.vue'
      import HelloWorld from './components/HelloWorld.vue'

      const routes = [
        { path: '/', component: CountriesList },
        { path: '/country', component: CountryForm },
        { path: '/hello', component: HelloWorld, props: { msg: 'Hello World desde ruta /hello' } }
      ]

      const router = createRouter({
        history: createWebHistory(),
        routes
      })

      createApp(App).use(router).mount('#app')
      ```
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
1. Me aseguro de que la base de datos y la tabla `Country` existen (puedo crear y consultar desde VS Code usando la extensión SQL Server).
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

## Notas

- Toda la administración y consulta de la base de datos la realicé desde Visual Studio Code usando la extensión SQL Server.
- El template de Web API en .NET 8 usa minimal APIs en `Program.cs` y no incluye la carpeta `Controllers` por defecto.
- Puedes agregar tus propios controladores y modelos según lo necesites.
- Recuerda configurar la cadena de conexión en `appsettings.json` para conectar tu API con SQL Server.

---