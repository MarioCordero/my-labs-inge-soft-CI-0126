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

        public async Task<IEnumerable<Coffee>> GetAllCoffeesAsync()
        {
            // Note: Yield() helps to mimic asynchronous behavior for in-memory operations. It can be removed when using a real database.
            await Task.Yield();
            var coffees = _db.CoffeeInventory.Select(kv => new Coffee
            {
                Name = kv.Key,
                Stock = kv.Value,
                PriceInCents = _db.CoffeePrices.GetValueOrDefault(kv.Key)
            }).ToList();
            return coffees;
        }

        public async Task<int> GetQuantityAsync(string coffeeName)
        {
            await Task.Yield();
            _db.CoffeeInventory.TryGetValue(coffeeName, out int quantity);
            return quantity;
        }

        public async Task<int> GetPriceInCentsAsync(string coffeeName)
        {
            await Task.Yield();
            _db.CoffeePrices.TryGetValue(coffeeName, out int price);
            return price;
        }

        public async Task<bool> UpdateInventoryAsync(Dictionary<string, int> purchasedItems)
        {
            await Task.Yield();
            foreach (var item in purchasedItems)
            {
                if (_db.CoffeeInventory.ContainsKey(item.Key))
                {
                    _db.CoffeeInventory[item.Key] -= item.Value;
                }
            }
            return true;
        }
    }
}