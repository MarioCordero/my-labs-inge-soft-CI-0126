using ExamTwo.Database;

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
            return Task.FromResult(new Dictionary<int, int>(_db.keyValues3));
        }

        public Task AddPaymentToInventoryAsync(Dictionary<int, int> paymentCoins)
        {   
            foreach (var coin in paymentCoins)
            {
                if (_db.keyValues3.ContainsKey(coin.Key))
                {
                    _db.keyValues3[coin.Key] += coin.Value;
                }
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
            var tempCoins = new Dictionary<int, int>(_db.keyValues3);
            foreach (var denomination in _db.SortedDenominations)
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
                foreach(var kvp in tempCoins)
                {
                    _db.keyValues3[kvp.Key] = kvp.Value;
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