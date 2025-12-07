using Xunit;
using Moq;
using ExamTwo.Services;
using ExamTwo.Repositories;
using ExamTwo.Models;
using Microsoft.Extensions.Logging;

namespace ExamTwo.Tests.Services
{
    public class CoinServiceTests
    {
        private readonly Mock<ICoinRepository> _mockCoinRepository;
        private readonly Mock<ILogger<CoinService>> _mockLogger;
        private readonly CoinService _coinService;

        public CoinServiceTests()
        {
            _mockCoinRepository = new Mock<ICoinRepository>();
            _mockLogger = new Mock<ILogger<CoinService>>();
            _coinService = new CoinService(_mockCoinRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetPaymentDenominationsAsync_ShouldReturnCoinsAndBills()
        {
            // Arrange
            var coins = new Dictionary<int, int> { { 500, 20 }, { 100, 30 } };
            var bills = new Dictionary<int, int> { { 1000, 10 } };

            _mockCoinRepository.Setup(r => r.GetAvailableCoinsAsync())
                .ReturnsAsync(coins);
            _mockCoinRepository.Setup(r => r.GetAvailableBillsAsync())
                .ReturnsAsync(bills);

            // Act
            var result = await _coinService.GetPaymentDenominationsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(coins, result.Coins);
            Assert.Equal(bills, result.Bills);
        }

        [Fact]
        public async Task GetPaymentDenominationsAsync_WithEmptyInventory_ShouldReturnEmptyDictionaries()
        {
            // Arrange
            _mockCoinRepository.Setup(r => r.GetAvailableCoinsAsync())
                .ReturnsAsync(new Dictionary<int, int>());
            _mockCoinRepository.Setup(r => r.GetAvailableBillsAsync())
                .ReturnsAsync(new Dictionary<int, int>());

            // Act
            var result = await _coinService.GetPaymentDenominationsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Coins);
            Assert.Empty(result.Bills);
        }
    }
}