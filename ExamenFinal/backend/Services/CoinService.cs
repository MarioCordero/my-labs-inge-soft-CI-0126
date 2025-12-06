using ExamTwo.Models;
using ExamTwo.Repositories;
using Microsoft.Extensions.Logging;

namespace ExamTwo.Services
{
    public class CoinService : ICoinService
    {
        private readonly ICoinRepository _coinRepository;
        private readonly ILogger<CoinService> _logger;

        public CoinService(ICoinRepository coinRepository, ILogger<CoinService> logger)
        {
            _coinRepository = coinRepository;
            _logger = logger;
        }

        public async Task<PaymentDenominations> GetPaymentDenominationsAsync()
        {
            var coinInventory = await _coinRepository.GetAvailableCoinsAsync();
            var billsInventory = await _coinRepository.GetAvailableBillsAsync();

            _logger.LogInformation("Fetching payment denominations. Coins: {@Coins}, Bills: {@Bills}", coinInventory, billsInventory);

            return new PaymentDenominations
            {
                Coins = coinInventory,
                Bills = billsInventory
            };
        }
    }
}