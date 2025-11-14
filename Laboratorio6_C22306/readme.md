# Laboratorio #6 - Vue.js + ASP.NET Core + Selenium UI Testing

## Descripción
En este laboratorio desarrollé una aplicación fullstack que conecta un frontend en Vue.js con un backend en ASP.NET Core, usando una API REST para gestionar países. Además, implementé **pruebas automatizadas de UI con Selenium WebDriver** para verificar el funcionamiento completo de la aplicación. Toda la consulta y administración de la base de datos la realicé directamente desde Visual Studio Code, usando extensiones para conectarme y ejecutar queries en SQL Server.

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

### Automatización UI con Selenium

**¿Qué agregué para las pruebas automatizadas?**

1. **Creé el proyecto de pruebas UIAutomationTests:**
    ```bash
    dotnet new nunit -n UIAutomationTests
    cd UIAutomationTests
    dotnet add package Selenium.WebDriver
    dotnet add package Selenium.WebDriver.ChromeDriver
    dotnet add package Selenium.Support
    ```

2. **Implementé 6 tests automatizados** en `CompleteSeleniumTests.cs`:
    - ✅ **HomePage_LoadsCorrectly**: Verifica que la página principal carga y muestra la tabla
    - ✅ **Navigation_ToCreateForm_Works**: Prueba la navegación al formulario de creación
    - ✅ **CreateCountry_Form_Validation**: Valida que los campos del formulario están presentes
    - ✅ **CreateNewCountry_Successfully**: Crea un país completo y verifica que aparece en la lista
    - ✅ **CountryTable_HasRequiredColumns**: Verifica las columnas de la tabla (Nombre, Continente, Idioma, Acciones)
    - ✅ **Page_Elements_AreInteractive**: Comprueba que los elementos de la página son interactivos

3. **Configuré ChromeDriver** con opciones optimizadas para Linux:
    ```csharp
    var chromeOptions = new ChromeOptions();
    chromeOptions.AddArgument("--no-sandbox");
    chromeOptions.AddArgument("--disable-dev-shm-usage");
    chromeOptions.AddArgument("--window-size=1920,1080");
    chromeOptions.AddArgument("--start-maximized");
    ```

4. **Implementé helpers robustos:**
    - `FindElementWithRetry()`: Busca elementos con múltiples selectores de fallback
    - `TryClick()`: Intenta hacer click con diferentes métodos (normal, Actions, JavaScript)
    - `TakeScreenshot()`: Captura pantallas para debug
    - `DumpPageSource()`: Guarda el HTML para análisis

5. **Sistema de artifacts**: Screenshots y HTML se guardan en `Docs/screenshots/` y `Docs/page-source/`

---

## Estructura del proyecto

```
Laboratorio6_C22306/
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
├── frontend-lab/
│   ├── src/
│   │   ├── components/
│   │   │   ├── CountriesList.vue
│   │   │   └── CountryForm.vue
│   │   └── main.js
│   └── ...
├── UIAutomationTests/
│   ├── CompleteSeleniumTests.cs
│   ├── UIAutomationTests.csproj
│   └── artifacts/
├── Docs/
│   ├── logs/
│   ├── screenshots/
│   └── page-source/
├── start-all.sh (🔥 NUEVO)
└── readme.md
```

---

## 🚀 ¿Cómo ejecutar todo? (Guía completa)

### Opción 1: Script automatizado (RECOMENDADO)

**Ejecuta todo de una vez:**
```bash
./start-all.sh
```

Este script:
1. ✅ Arranca el **backend** en background
2. ✅ Arranca el **frontend** en background  
3. ✅ Espera a que ambos servicios respondan
4. ✅ Ejecuta las **6 pruebas de Selenium** automáticamente
5. ✅ Guarda logs en `Docs/logs/`
6. ✅ Guarda screenshots en `Docs/screenshots/`
7. ✅ Limpia procesos al terminar

**Salida esperada:**
```
Starting frontend...
Frontend PID: 1234 (logs: Docs/logs/frontend.log)
Starting backend...
Backend PID: 5678 (logs: Docs/logs/backend.log)
Waiting for frontend (http://localhost:8080/) ...
OK: http://localhost:8080/
Waiting for backend (ports 5000/5001) or log readiness...
Backend ready.
Running UI tests...
✅ 5/6 tests passed
```

### Opción 2: Paso a paso (Manual)

**1. Backend:**
```bash
cd backend-lab
dotnet run
# Debe mostrar: https://localhost:7019
```

**2. Frontend (en otra terminal):**
```bash
cd frontend-lab
npm install
npm run serve
# Debe mostrar: http://localhost:8080
```

**3. Pruebas Selenium (en tercera terminal):**
```bash
cd UIAutomationTests
dotnet test --logger "console;verbosity=detailed"
```

---

## Validaciones y pruebas automatizadas

### Tests implementados:
- **Carga de página**: Verifica título, tabla y columnas
- **Navegación**: Prueba el botón "Agregar país" → formulario
- **Formulario**: Valida campos (name, language, continent) y botón submit
- **Creación completa**: Llena formulario → envía → verifica país en lista
- **Estructura de tabla**: Confirma columnas requeridas
- **Interactividad**: Cuenta elementos clickeables

### Manejo de errores en tests:
- **Screenshots automáticos** cuando falla un test
- **Page source HTML** guardado para debugging
- **Selectores múltiples** para mayor robustez
- **Timeouts configurables** para esperar carga de Vue.js
- **Limpieza automática** de procesos ChromeDriver

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

## Debugging y troubleshooting

### Si fallan las pruebas:
1. **Revisa screenshots**: `Docs/screenshots/`
2. **Revisa HTML**: `Docs/page-source/`  
3. **Revisa logs de servicios**: `Docs/logs/`
4. **Ejecuta tests en modo visible**: Comenta `--headless` en ChromeOptions

### Comandos útiles:
```bash
# Ver logs en tiempo real
tail -f Docs/logs/frontend.log
tail -f Docs/logs/backend.log

# Abrir última captura
xdg-open Docs/screenshots/$(ls -t Docs/screenshots | head -n1)

# Limpiar artifacts
rm -rf Docs/screenshots/* Docs/page-source/*

# Solo tests (sin servicios)
cd UIAutomationTests && dotnet test
```

### Requisitos del sistema:
- ✅ .NET 8 SDK
- ✅ Node.js + npm  
- ✅ Google Chrome
- ✅ ChromeDriver (se descarga automáticamente)
- ✅ curl (para health checks)

---

## Resultados típicos

**Tests exitosos:**
- ✅ HomePage_LoadsCorrectly
- ✅ Navigation_ToCreateForm_Works  
- ✅ CreateCountry_Form_Validation
- ✅ CreateNewCountry_Successfully
- ✅ CountryTable_HasRequiredColumns
- ✅ Page_Elements_AreInteractive

**Total: 6 tests, ~4 minutos de ejecución**

---

## Notas técnicas

- **ChromeDriver**: Se ejecuta en modo visible por defecto (útil para debug)
- **Esperas inteligentes**: WebDriverWait para elementos dinámicos de Vue.js
- **Reutilización de navegador**: OneTimeSetUp para mejor rendimiento
- **Cross-platform**: Configurado para Linux con `--no-sandbox`
- **CI/CD ready**: Puede ejecutarse en headless descomentando la opción

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