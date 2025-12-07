# Coffee Machine - Sistema de Máquina Expendedora de Café

Un sistema completo de máquina expendedora de café con backend en ASP.NET Core 8 y frontend en Vue.js 3.

## 📋 Descripción General

El proyecto implementa una máquina expendedora de café con las siguientes características:

- **Catálogo de Cafés**: Consultar cafés disponibles con precios y stock
- **Carrito de Compras**: Agregar múltiples cafés a un pedido
- **Sistema de Pago**: Ingreso de monedas y billetes
- **Cálculo de Cambio**: Dispensación automática de cambio usando algoritmo voraz
- **Gestión de Inventario**: Control de stock de cafés y monedas
- **Unit Tests**: Pruebas automatizadas de servicios y repositorios

## 🏗️ Arquitectura

### Backend (ASP.NET Core 8)
```
backend/
├── Controllers/          # Endpoints HTTP
├── Services/            # Lógica de negocio
├── Repositories/        # Acceso a datos
├── Models/             # Modelos de datos
└── Database/           # Base de datos en memoria
```

### Frontend (Vue.js 3)
```
frontend-vue/
├── src/
│   ├── components/     # Componentes Vue
│   ├── composables/    # Composables (hooks)
│   ├── lib/           # Utilidades
│   └── App.vue        # Componente principal
```

### Tests (xUnit + Moq)
```
ExamTwo.Tests
├── Services/          # Tests de servicios
├── Repositories/      # Tests de repositorios
└── Controllers/       # Tests de endpoints
```

## 🚀 Instalación y Compilación

### Requisitos Previos
- **.NET 8 SDK**: [Descargar](https://dotnet.microsoft.com/download)
- **Node.js 18+**: [Descargar](https://nodejs.org/)
- **Git**: Para clonar el repositorio

### Backend

```bash
# 1. Navegar a la carpeta del proyecto
cd ExamenFinal

# 2. Restaurar paquetes NuGet
dotnet restore backend

# 3. Compilar el backend
dotnet build backend

# 4. Ejecutar el backend
dotnet run --project backend

# El servidor estará disponible en: http://localhost:5011
```

### Frontend

```bash
# 1. Navegar a la carpeta del frontend
cd frontend-vue

# 2. Instalar dependencias
npm install

# 3. Ejecutar en modo desarrollo
npm run dev

# El frontend estará disponible en: http://localhost:5173
```

### Tests

```bash
# 1. Navegar a la raíz del proyecto
cd ExamenFinal

# 2. Ejecutar todos los tests
dotnet test

# 3. Ejecutar con detalles
dotnet test --verbosity detailed

# 4. Ejecutar solo un archivo de tests
dotnet test ExamTwo.Tests/Services/CoffeeServiceTests.cs

# 5. Ver cobertura de código
dotnet test /p:CollectCoverageMetrics=true
```

## 📚 Endpoints API

### Coffee Management
```
GET /getCoffees
Retorna lista de todos los cafés disponibles

Response:
[
  {
    "name": "Americano",
    "priceInCents": 950,
    "stock": 10
  }
]
```

### Payment Denominations
```
GET /getPaymentDenominations
Retorna monedas y billetes disponibles en la máquina

Response:
{
  "coins": { "500": 20, "100": 30 },
  "bills": { "1000": 10 }
}
```

### Buy Coffee
```
POST /buyCoffee
Procesa una compra de café

Request:
{
  "order": { "Americano": 1, "Latte": 2 },
  "totalPayment": 3100,
  "payment": {
    "coins": [500, 100],
    "bills": [1000, 1000, 500]
  }
}

Response (Success):
{
  "code": 0,
  "message": "Su vuelto es de: 600 colones. Desglose: 1 moneda de 500, 2 moneda de 50",
  "changeAmount": 600,
  "changeBreakdown": { "500": 1, "50": 2 }
}

Response (Error):
{
  "code": 99,
  "message": "No hay suficiente stock. para 'Mocaccino'. Stock disponible: 14."
}
```

## 🧪 Tests Incluidos

### CoffeeServiceTests (5 tests)
- ✅ `GetCoffeeOptionsAsync_ShouldReturnAllCoffees`
- ✅ `ProcessPurchaseAsync_WithValidOrder_ShouldSucceed`
- ✅ `ProcessPurchaseAsync_WithInsufficientFunds_ShouldFail`
- ✅ `ProcessPurchaseAsync_WithOutOfStock_ShouldFail`
- ✅ `ProcessPurchaseAsync_WithNullOrder_ShouldFail`

### CoinServiceTests (2 tests)
- ✅ `GetPaymentDenominationsAsync_ShouldReturnCoinsAndBills`
- ✅ `GetPaymentDenominationsAsync_WithEmptyInventory_ShouldReturnEmptyDictionaries`

### CoinRepositoryTests (5 tests)
- ✅ `GetAvailableCoinsAsync_ShouldReturnCoinInventory`
- ✅ `GetAvailableBillsAsync_ShouldReturnBillInventory`
- ✅ `TryDispenseChangeAsync_WithExactChange_ShouldSucceed`
- ✅ `TryDispenseChangeAsync_WithZeroAmount_ShouldReturnEmpty`
- ✅ `AddPaymentToInventoryAsync_ShouldAddCoinsAndBills`

## 🔧 Configuración

### Variables de Entorno (Frontend)

Crear `.env` en `frontend-vue`:
```
VITE_API_BASE_URL=http://localhost:5011
```

### CORS (Backend)

El backend permite peticiones desde `http://localhost:5173` (frontend en desarrollo).

## 📂 Estructura de Carpetas

```
ExamenFinal/
├── backend/
│   ├── Controllers/
│   │   └── CoffeeMachineController.cs
│   ├── Services/
│   │   ├── ICoffeeService.cs
│   │   ├── CoffeeService.cs
│   │   ├── ICoinService.cs
│   │   └── CoinService.cs
│   ├── Repositories/
│   │   ├── ICoffeeRepository.cs
│   │   ├── CoffeeRepository.cs
│   │   ├── ICoinRepository.cs
│   │   └── CoinRepository.cs
│   ├── Models/
│   │   ├── Coffee.cs
│   │   ├── OrderRequest.cs
│   │   ├── ChangeResult.cs
│   │   ├── PaymentDenominations.cs
│   │   └── CoffeeMachineError.cs
│   ├── Database/
│   │   └── Database.cs
│   └── Program.cs
├── frontend-vue/
│   ├── src/
│   │   ├── components/
│   │   │   ├── CoffeeCard.vue
│   │   │   └── OrderPanel.vue
│   │   ├── composables/
│   │   │   └── useCart.js
│   │   ├── lib/
│   │   │   ├── apiConfig.js
│   │   │   └── utils.js
│   │   ├── App.vue
│   │   └── main.js
│   ├── index.html
│   ├── package.json
│   └── vite.config.js
├── ExamTwo.Tests/
│   ├── Services/
│   │   ├── CoffeeServiceTests.cs
│   │   └── CoinServiceTests.cs
│   ├── Repositories/
│   │   └── CoinRepositoryTests.cs
│   └── ExamTwo.Tests.csproj
├── ExamenFinal.sln
└── README.md
```

## 🛠️ Tecnologías Utilizadas

### Backend
- **ASP.NET Core 8**: Framework web
- **C# 12**: Lenguaje de programación
- **xUnit**: Framework de testing
- **Moq**: Librería de mocking

### Frontend
- **Vue.js 3**: Framework frontend
- **Vite**: Bundler
- **Tailwind CSS**: Estilos
- **JavaScript ES6+**: Lenguaje

### Base de Datos
- **En Memoria**: Simulación de BD para desarrollo

## 📝 Notas Importantes

1. **Base de Datos**: Actualmente usa una BD en memoria. Los datos se pierden al reiniciar.
2. **CORS**: Configurado solo para desarrollo. En producción, cambiar orígenes permitidos.
3. **Validación**: El backend valida todos los datos de entrada.
4. **Errores**: Se manejan con códigos de error específicos.

## 🐛 Troubleshooting

### Backend no inicia
```bash
# Limpiar caché de build
rm -rf backend/bin backend/obj

# Restaurar y compilar de nuevo
dotnet restore backend
dotnet build backend
```

### Frontend no carga
```bash
# Limpiar node_modules
rm -rf frontend-vue/node_modules package-lock.json

# Reinstalar
cd frontend-vue
npm install
npm run dev
```

### Tests no se ejecutan
```bash
# Limpiar y restaurar
pkill -f dotnet
rm -rf ExamTwo.Tests/bin ExamTwo.Tests/obj
dotnet restore ExamTwo.Tests
dotnet test
```

## 📧 Autor

Mario Cordero
Proyecto de Ingeniería de Software - Universidad de Costa Rica