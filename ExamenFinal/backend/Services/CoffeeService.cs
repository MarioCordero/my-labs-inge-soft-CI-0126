using ExamTwo.Models;
using ExamTwo.Repositories;

namespace ExamTwo.Services
{
    public class CoffeeService : ICoffeeService
    {
        private readonly ICoffeeRepository _coffeeRepository;
        private readonly ICoinRepository _coinRepository;
        public CoffeeService(ICoffeeRepository coffeeRepository, ICoinRepository coinRepository)
        {
            _coffeeRepository = coffeeRepository;
            _coinRepository = coinRepository;
        }

        public async Task<IEnumerable<Coffee>> GetCoffeeOptionsAsync()
        {
            return await _coffeeRepository.GetAllCoffeesAsync();
        }

        public Task<ChangeResult> ProcessPurchaseAsync(OrderRequest request)
        { // MOCK FUNCTION
            var result = new ChangeResult
            {
                ErrorMessage = null,
                ChangeBreakdown = new Dictionary<int, int>()
            };
            return Task.FromResult(result);
        }
    }
}