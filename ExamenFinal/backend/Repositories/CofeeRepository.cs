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
            var coffees = _db.CoffeeInventory.Select(kv => new Coffee
            {
                Name = kv.Key,
                Stock = kv.Value,
                PriceInCents = _db.CoffeePrices.GetValueOrDefault(kv.Key)
            }).ToList();
            return Task.FromResult<IEnumerable<Coffee>>(coffees);
        }

        public Task<int> GetQuantityAsync(string coffeeName)
        {
            _db.CoffeeInventory.TryGetValue(coffeeName, out int quantity);
            return Task.FromResult(quantity);
        }

        public Task<int> GetPriceInCentsAsync(string coffeeName)
        {
            _db.CoffeePrices.TryGetValue(coffeeName, out int price);
            return Task.FromResult(price);
        }

        public Task<bool> UpdateInventoryAsync(Dictionary<string, int> purchasedItems)
        {
            foreach (var item in purchasedItems)
            {
                if (_db.CoffeeInventory.ContainsKey(item.Key))
                {
                    _db.CoffeeInventory[item.Key] -= item.Value;
                }
            }
            return Task.FromResult(true);
        }
    }
}