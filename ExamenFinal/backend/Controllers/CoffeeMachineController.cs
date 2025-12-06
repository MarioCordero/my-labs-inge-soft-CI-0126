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
        public ActionResult<IEnumerable<Coffee>> GetCoffeeOptions()
        {
            var coffees = _coffeeService.GetCoffeeOptions(); 
            return Ok(coffees);
        }

        [HttpPost("buyCoffee")]
        public ActionResult<string> BuyCoffee([FromBody] OrderRequest request)
        {
            if (request == null || request.Order == null || request.Order.Count == 0)
                return BadRequest("Orden vacía.");

            try
            {
                ChangeResult result = _coffeeService.ProcessPurchase(request);

                if (result.IsSuccess)
                {
                    return Ok(result.GetFormattedChangeMessage());
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del sistema: " + ex.Message);
            }
        }
    }
}