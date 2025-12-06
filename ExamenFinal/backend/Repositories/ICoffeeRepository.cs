using ExamTwo.Models; 

namespace ExamTwo.Repositories
{
    public interface ICoffeeRepository
    {
        Task<IEnumerable<Coffee>> GetAllCoffeesAsync();
        
        Task<int> GetQuantityAsync(string coffeeName);
        
        Task<int> GetPriceInCentsAsync(string coffeeName);
        
        Task<bool> UpdateInventoryAsync(Dictionary<string, int> purchasedItems);
    }
}