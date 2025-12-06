namespace ExamTwo.Repositories
{
    public interface ICoinRepository
    {
        Task<Dictionary<int, int>> GetAvailableCoinsAsync();
        
        Task<Dictionary<int, int>?> TryDispenseChangeAsync(int amountNeeded);
        
        Task AddPaymentToInventoryAsync(Dictionary<int, int> paymentCoins);
    }
}