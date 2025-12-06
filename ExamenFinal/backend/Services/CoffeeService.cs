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

        public async Task<ChangeResult> ProcessPurchaseAsync(OrderRequest request)
        {
            var result = new ChangeResult
            {
                ChangeBreakdown = new Dictionary<int, int>()
            };

            var checkResult = await CheckOrder(request, result);
            if (!checkResult.IsSuccess)
                return checkResult;

            int price = await CalculateTotalCostAsync(request);
            int change = request.TotalPayment - price;
            var dispenseResult = await CheckDispense(change, result);
            if(!dispenseResult.IsSuccess)
                return dispenseResult;

            await _coinRepository.AddPaymentToInventoryAsync(request);
            await _coffeeRepository.UpdateInventoryAsync(request.Order);
            return result;
        }

        private async Task<ChangeResult> CheckDispense(int change, ChangeResult result)
        {
            var dispenseResult = await _coinRepository.TryDispenseChangeAsync(change);

            if (change < 0)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "Pago insuficiente.";
                return result;
            }
            if (dispenseResult == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "No hay suficiente cambio disponible.";
                return result;
            }
            if(dispenseResult.Count == 0 && change > 0)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "No hay suficiente cambio disponible.";
                return result;
            }
            result.IsSuccess = true;
            result.ErrorMessage = "";
            return result;
        }

        private async Task<int> CalculateTotalCostAsync(OrderRequest request)
        {
            int totalCost = 0;
            foreach (var item in request.Order)
            {
                var price = await _coffeeRepository.GetPriceInCentsAsync(item.Key);
                totalCost += price * item.Value;
            }
            return totalCost;
        }
        private async Task<ChangeResult> CheckOrder(OrderRequest request, ChangeResult result)
        {

            var coinInventory = await _coinRepository.GetAvailableCoinsAsync();
            var coffeeOptions = await _coffeeRepository.GetAllCoffeesAsync();

            if (coffeeOptions == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "No hay cafés disponibles.";
                return result;
            }
            if (request.Order == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "Error, la solicitud de pedido está vacía.";
                return result;
            }
            foreach (var item in request.Order)
            {
                var coffee = coffeeOptions.FirstOrDefault(c => c.Name == item.Key);
                if (coffee == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = $"El café '{item.Key}' no existe.";
                    return result;
                }
                if (item.Value <= 0)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = $"La cantidad solicitada para '{item.Key}' debe ser mayor que cero.";
                    return result;
                }
                if (coffee.Stock < item.Value)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = $"No hay suficiente stock para '{item.Key}'. Stock disponible: {coffee.Stock}.";
                    return result;
                }
            }
            result.IsSuccess = true;
            result.ErrorMessage = "";
            return result;
        }
    }
}