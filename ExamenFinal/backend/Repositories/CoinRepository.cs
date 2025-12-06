using ExamTwo.Database;
using ExamTwo.Models;

namespace ExamTwo.Repositories
{
    public class CoinRepository : ICoinRepository
    {
        private readonly DatabaseMostra _db;
        public CoinRepository(DatabaseMostra db)
        {
            _db = db;
        }
        
        public Task<Dictionary<int, int>> GetAvailableCoinsAsync()
        {
            return Task.FromResult(new Dictionary<int, int>(_db.CoinInventory));
        }

        public Task AddPaymentToInventoryAsync(PaymentDetails payment)
        {
            // Lists to dictionaries with counts
            var coinDict = payment.Coins
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            var billDict = payment.Bills != null
                ? payment.Bills.GroupBy(b => b).ToDictionary(g => g.Key, g => g.Count())
                : new Dictionary<int, int>();

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