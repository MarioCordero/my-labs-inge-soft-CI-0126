using ExamTwo.Models;
using ExamTwo.Repositories;
using Microsoft.Extensions.Logging;

namespace ExamTwo.Services
{
    public class CoffeeService : ICoffeeService
    {
        private readonly ICoffeeRepository _coffeeRepository;
        private readonly ICoinRepository _coinRepository;
        private readonly ILogger<CoffeeService> _logger;

        public CoffeeService(ICoffeeRepository coffeeRepository, ICoinRepository coinRepository, ILogger<CoffeeService> logger)
        {
            _coffeeRepository = coffeeRepository;
            _coinRepository = coinRepository;
            _logger = logger;
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
            int totalPaid = await GetTotalPaid(request);

            var paymentCheck = CheckPayment(totalPaid, price);
            if (!paymentCheck.IsSuccess)
                return paymentCheck;

            int change = totalPaid - price;
            _logger.LogInformation("Total paid: {TotalPaid}, Price: {Price}, Change: {Change}", totalPaid, price, change);

            var changeCheck = await CheckAndDispenseChange(change);
            if (!changeCheck.IsSuccess)
                return changeCheck;

            await _coinRepository.AddPaymentToInventoryAsync(request.Payment);
            await _coffeeRepository.UpdateInventoryAsync(request.Order);

            result.IsSuccess = true;
            result.ErrorCode = CoffeeMachineErrorCode.None;
            result.ErrorMessage = string.Empty;
            result.ChangeAmount = change;
            result.ChangeBreakdown = changeCheck.ChangeBreakdown;
            return result;
        }

        private async Task<ChangeResult> CheckOrder(OrderRequest request, ChangeResult result)
        {
            var coffeeOptions = await _coffeeRepository.GetAllCoffeesAsync();
            if (coffeeOptions == null)
            {
                result.IsSuccess = false;
                result.ErrorCode = CoffeeMachineErrorCode.InternalError;
                result.ErrorMessage = CoffeeMachineErrorMessages.InternalError;
                return result;
            }
            if (request.Order == null || request.Order.Count == 0)
            {
                result.IsSuccess = false;
                result.ErrorCode = CoffeeMachineErrorCode.EmptyOrder;
                result.ErrorMessage = CoffeeMachineErrorMessages.EmptyOrder;
                return result;
            }
            foreach (var item in request.Order)
            {
                var coffee = coffeeOptions.FirstOrDefault(c => c.Name == item.Key);
                if (coffee == null)
                {
                    result.IsSuccess = false;
                    result.ErrorCode = CoffeeMachineErrorCode.OutOfStock;
                    result.ErrorMessage = $"El café '{item.Key}' no existe.";
                    return result;
                }
                if (item.Value <= 0)
                {
                    result.IsSuccess = false;
                    result.ErrorCode = CoffeeMachineErrorCode.InvalidPayment;
                    result.ErrorMessage = CoffeeMachineErrorMessages.InvalidPayment;
                    return result;
                }
                if (coffee.Stock < item.Value)
                {
                    result.IsSuccess = false;
                    result.ErrorCode = CoffeeMachineErrorCode.OutOfStock;
                    result.ErrorMessage = $"{CoffeeMachineErrorMessages.OutOfStock} para '{item.Key}'. Stock disponible: {coffee.Stock}.";
                    return result;
                }
            }
            result.IsSuccess = true;
            return result;
        }

        private async Task<int> GetTotalPaid(OrderRequest request)
        {
            int totalPaid = 0;
            if (request.Payment != null)
            {
                totalPaid += request.Payment.Coins?.Sum() ?? 0;
                totalPaid += request.Payment.Bills?.Sum() ?? 0;
            }
            return totalPaid;
        }

        private ChangeResult CheckPayment(int totalPaid, int price)
        {
            var result = new ChangeResult();
            if (totalPaid < price)
            {
                result.IsSuccess = false;
                result.ErrorCode = CoffeeMachineErrorCode.InvalidPayment;
                result.ErrorMessage = CoffeeMachineErrorMessages.InvalidPayment;
                return result;
            }
            result.IsSuccess = true;
            return result;
        }

        private async Task<ChangeResult> CheckAndDispenseChange(int change)
        {
            var result = new ChangeResult();
            var changeBreakdown = await _coinRepository.TryDispenseChangeAsync(change);

            if (change < 0)
            {
                result.IsSuccess = false;
                result.ErrorCode = CoffeeMachineErrorCode.InvalidPayment;
                result.ErrorMessage = CoffeeMachineErrorMessages.InvalidPayment;
                return result;
            }
            if (changeBreakdown == null || (changeBreakdown.Count == 0 && change > 0))
            {
                result.IsSuccess = false;
                result.ErrorCode = CoffeeMachineErrorCode.InvalidPayment;
                result.ErrorMessage = "No hay suficiente cambio disponible.";
                return result;
            }
            result.IsSuccess = true;
            result.ChangeBreakdown = changeBreakdown;
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
    }
}