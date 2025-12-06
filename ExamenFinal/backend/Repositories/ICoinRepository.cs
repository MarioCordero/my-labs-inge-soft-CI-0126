using ExamTwo.Models;

namespace ExamTwo.Repositories
{
    public interface ICoinRepository
    {
        Task<Dictionary<int, int>> GetAvailableCoinsAsync();
        
        Task<Dictionary<int, int>?> TryDispenseChangeAsync(int amountNeeded);
        
        Task AddPaymentToInventoryAsync(PaymentDetails payment);
    }
}