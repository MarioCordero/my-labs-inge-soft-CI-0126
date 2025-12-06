using ExamTwo.Models;
using ExamTwo.Database;

namespace ExamTwo.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly DatabaseMostra _db;

        public CoffeeRepository(DatabaseMostra db)
        {
            _db = db;
        }

        public Task<IEnumerable<Coffee>> GetAllCoffeesAsync()
        {
            var coffees = _db.keyValues.Select(kv => new Coffee
            {
                Name = kv.Key,
                Stock = kv.Value,
                PriceInCents = _db.keyValues2.GetValueOrDefault(kv.Key)
            }).ToList();
            return Task.FromResult<IEnumerable<Coffee>>(coffees);
        }

        public Task<int> GetQuantityAsync(string coffeeName)
        {
            _db.keyValues.TryGetValue(coffeeName, out int quantity);
            return Task.FromResult(quantity);
        }

        public Task<int> GetPriceInCentsAsync(string coffeeName)
        {
            _db.keyValues2.TryGetValue(coffeeName, out int price);
            return Task.FromResult(price);
        }

        public Task<bool> UpdateInventoryAsync(Dictionary<string, int> purchasedItems)
        {
            foreach (var item in purchasedItems)
            {
                if (_db.keyValues.ContainsKey(item.Key))
                {
                    _db.keyValues[item.Key] -= item.Value;
                }
            }
            return Task.FromResult(true);
        }
    }
}