using Xunit;
using Moq;
using ExamTwo.Services;
using ExamTwo.Repositories;
using ExamTwo.Models;
using Microsoft.Extensions.Logging;

namespace ExamTwo.Tests.Services
{
    public class CoffeeServiceTests
    {
        private readonly Mock<ICoffeeRepository> _mockCoffeeRepository;
        private readonly Mock<ICoinRepository> _mockCoinRepository;
        private readonly Mock<ILogger<CoffeeService>> _mockLogger;
        private readonly CoffeeService _coffeeService;

        public CoffeeServiceTests()
        {
            _mockCoffeeRepository = new Mock<ICoffeeRepository>();
            _mockCoinRepository = new Mock<ICoinRepository>();
            _mockLogger = new Mock<ILogger<CoffeeService>>();
            _coffeeService = new CoffeeService(_mockCoffeeRepository.Object, _mockCoinRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCoffeeOptionsAsync_ShouldReturnAllCoffees()
        {
            // Arrange
            var expectedCoffees = new List<Coffee>
            {
                new Coffee { Name = "Americano", PriceInCents = 950, Stock = 10 },
                new Coffee { Name = "Cappuccino", PriceInCents = 1200, Stock = 8 }
            };
            _mockCoffeeRepository.Setup(r => r.GetAllCoffeesAsync())
                .ReturnsAsync(expectedCoffees);

            // Act
            var result = await _coffeeService.GetCoffeeOptionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Americano");
        }

        [Fact]
        public async Task ProcessPurchaseAsync_WithValidOrder_ShouldSucceed()
        {
            // Arrange
            var order = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 1 } },
                TotalPayment = 1000,
                Payment = new PaymentDetails
                {
                    Coins = new List<int> { 500, 500 },
                    Bills = new List<int>()
                }
            };

            _mockCoffeeRepository.Setup(r => r.GetAllCoffeesAsync())
                .ReturnsAsync(new List<Coffee>
                {
                    new Coffee { Name = "Americano", PriceInCents = 950, Stock = 10 }
                });

            _mockCoffeeRepository.Setup(r => r.GetPriceInCentsAsync("Americano"))
                .ReturnsAsync(950);

            _mockCoinRepository.Setup(r => r.TryDispenseChangeAsync(It.IsAny<int>()))
                .ReturnsAsync(new Dictionary<int, int> { { 50, 1 } });

            // Act
            var result = await _coffeeService.ProcessPurchaseAsync(order);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(50, result.ChangeAmount);
        }

        [Fact]
        public async Task ProcessPurchaseAsync_WithInsufficientFunds_ShouldFail()
        {
            // Arrange
            var order = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 1 } },
                TotalPayment = 500,
                Payment = new PaymentDetails
                {
                    Coins = new List<int> { 500 },
                    Bills = new List<int>()
                }
            };

            _mockCoffeeRepository.Setup(r => r.GetAllCoffeesAsync())
                .ReturnsAsync(new List<Coffee>
                {
                    new Coffee { Name = "Americano", PriceInCents = 950, Stock = 10 }
                });

            _mockCoffeeRepository.Setup(r => r.GetPriceInCentsAsync("Americano"))
                .ReturnsAsync(950);

            // Act
            var result = await _coffeeService.ProcessPurchaseAsync(order);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(CoffeeMachineErrorCode.InvalidPayment, result.ErrorCode);
        }

        [Fact]
        public async Task ProcessPurchaseAsync_WithOutOfStock_ShouldFail()
        {
            // Arrange
            var order = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 5 } },
                TotalPayment = 5000,
                Payment = new PaymentDetails
                {
                    Coins = new List<int>(),
                    Bills = new List<int> { 5000 }
                }
            };

            _mockCoffeeRepository.Setup(r => r.GetAllCoffeesAsync())
                .ReturnsAsync(new List<Coffee>
                {
                    new Coffee { Name = "Americano", PriceInCents = 950, Stock = 2 }
                });

            // Act
            var result = await _coffeeService.ProcessPurchaseAsync(order);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(CoffeeMachineErrorCode.OutOfStock, result.ErrorCode);
        }

        [Fact]
        public async Task ProcessPurchaseAsync_WithNullOrder_ShouldFail()
        {
            // Arrange
            var order = new OrderRequest
            {
                Order = null,
                TotalPayment = 1000,
                Payment = new PaymentDetails()
            };

            // Act
            var result = await _coffeeService.ProcessPurchaseAsync(order);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(CoffeeMachineErrorCode.EmptyOrder, result.ErrorCode);
        }
    }
}