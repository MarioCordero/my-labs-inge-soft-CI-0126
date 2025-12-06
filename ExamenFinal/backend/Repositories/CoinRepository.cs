using ExamTwo.Database;
using ExamTwo.Models;
using Microsoft.Extensions.Logging;

namespace ExamTwo.Repositories
{
    public class CoinRepository : ICoinRepository
    {
        private readonly DatabaseMostra _db;
        private readonly ILogger<CoinRepository> _logger;

        public CoinRepository(DatabaseMostra db, ILogger<CoinRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        
        public Task<Dictionary<int, int>> GetAvailableCoinsAsync()
        {
            return Task.FromResult(new Dictionary<int, int>(_db.CoinInventory));
        }
        public Task<Dictionary<int, int>> GetAvailableBillsAsync()
        {
            return Task.FromResult(new Dictionary<int, int>(_db.BillsInventory));
        }

        public async Task<PaymentDenominations> GetPaymentDenominationsAsync()
        {
            var coinInventory = await GetAvailableCoinsAsync();
            var billsInventory = await GetAvailableBillsAsync();
            return new PaymentDenominations
            {
                Coins = coinInventory,
                Bills = billsInventory
            };
        }

        public Task AddPaymentToInventoryAsync(PaymentDetails payment)
        {
            if (payment == null)
                return Task.CompletedTask;

            // Lists to dictionaries with counts
            var coinDict = (payment.Coins ?? new List<int>())
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            var billDict = (payment.Bills ?? new List<int>())
                .GroupBy(b => b)
                .ToDictionary(g => g.Key, g => g.Count());

            // Add coins
            foreach (var coin in coinDict)
            {
                if (_db.CoinInventory.ContainsKey(coin.Key))
                    _db.CoinInventory[coin.Key] += coin.Value;
                else
                    _db.CoinInventory[coin.Key] = coin.Value;
            }

            // Add bills
            foreach (var bill in billDict)
            {
                if (_db.BillsInventory.ContainsKey(bill.Key))
                    _db.BillsInventory[bill.Key] += bill.Value;
                else
                    _db.BillsInventory[bill.Key] = bill.Value;
            }

            return Task.CompletedTask;
        }

        public Task<Dictionary<int, int>?> TryDispenseChangeAsync(int amountNeeded)
        {
            if (amountNeeded <= 0)
            {
                return Task.FromResult<Dictionary<int, int>?>(new Dictionary<int, int>());
            }
            int remainingChange = amountNeeded;
            Dictionary<int, int> changeBreakdown = new Dictionary<int, int>();
            var tempCoins = new Dictionary<int, int>(_db.CoinInventory);
            var denominations = tempCoins.Keys.OrderByDescending(x => x).ToList();

            foreach (var denomination in denominations)
            {
                if (remainingChange == 0) break;

                int required = remainingChange / denomination;
                int available = tempCoins.GetValueOrDefault(denomination);

                int dispensedCount = Math.Min(required, available);

                if (dispensedCount > 0)
                {
                    changeBreakdown.Add(denomination, dispensedCount);
                    remainingChange -= dispensedCount * denomination;
                    tempCoins[denomination] -= dispensedCount;
                }
            }

            if (remainingChange == 0)
            {
                foreach (var kvp in tempCoins)
                {
                    _db.CoinInventory[kvp.Key] = kvp.Value;
                }
                return Task.FromResult<Dictionary<int, int>?>(changeBreakdown);
            }
            else
            {
                return Task.FromResult<Dictionary<int, int>?>(null);
            }
        }
    }
}