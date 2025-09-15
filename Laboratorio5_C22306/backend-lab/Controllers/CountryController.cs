using Microsoft.AspNetCore.Mvc;
using backend_lab_C22306.Models;
using backend_lab_C22306.Services; // <-- Agrega este using

namespace backend_lab_C22306.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly CountryService _countryService;

        public CountryController(CountryService countryService)// <-- Agrega este constructor
        {
            _countryService = countryService;
        }

        [HttpGet]
        public string Get()
        {
            return "Hola Mundo";
        }

        [HttpPost]
        public IActionResult CreateCountry([FromBody] CountryModel country)
        {
            if (country == null)
                return BadRequest("El país no puede ser nulo");

            var result = _countryService.CreateCountry(country);
            if (string.IsNullOrEmpty(result))
                return Ok(true);
            else
                return BadRequest(result);
        }
    }
}