using Microsoft.AspNetCore.Mvc;
using ExamTwo.Services;
using ExamTwo.Models;

namespace ExamTwo.Controllers
{
    public class CoffeeMachineController : Controller
    {
        private readonly ICoffeeService _coffeeService; 

        public CoffeeMachineController(ICoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }

        [HttpGet("getCoffees")]
        public async Task<ActionResult<IEnumerable<Coffee>>> GetCoffeeOptions()
        {
            var coffees = await _coffeeService.GetCoffeeOptionsAsync(); 
            return Ok(coffees);
        }

        [HttpGet("getPaymentDenominations")]
        public async Task<ActionResult<PaymentDenominations>> GetPaymentDenominations()
        {
            var denominations = await _coffeeService.GetPaymentDenominationsAsync();
            return Ok(denominations);
        }

        [HttpPost("buyCoffee")]
        public async Task<ActionResult<string>> BuyCoffee([FromBody] OrderRequest request)
        {
            if (request == null || request.Order == null || request.Order.Count == 0)
                return BadRequest(new
                {
                    Code = CoffeeMachineErrorCode.EmptyOrder,
                    Message = CoffeeMachineErrorMessages.EmptyOrder
                });

            try
            {
                ChangeResult result = await _coffeeService.ProcessPurchaseAsync(request);

                if (result.IsSuccess)
                {
                    return Ok(new
                    {
                        code = CoffeeMachineErrorCode.None,
                        message = result.GetFormattedChangeMessage(),
                        changeAmount = result.ChangeAmount,
                        changeBreakdown = result.ChangeBreakdown
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Code = CoffeeMachineErrorCode.InternalError,
                        Message = result.ErrorMessage ?? CoffeeMachineErrorMessages.InternalError
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Code = CoffeeMachineErrorCode.InternalError,
                    Message = CoffeeMachineErrorMessages.InternalError + ": " + ex.Message
                });
            }
        }
    }
}