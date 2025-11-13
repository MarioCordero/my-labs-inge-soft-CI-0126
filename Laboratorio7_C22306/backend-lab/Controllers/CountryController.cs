using Microsoft.AspNetCore.Mvc;
using backend_lab_C22306.Models;
using backend_lab_C22306.Services;

namespace backend_lab_C22306.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly CountryService _countryService;

        public CountryController(CountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var countries = _countryService.GetCountries();
            return Ok(countries);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CountryModel country)
        {
            if (country == null || string.IsNullOrEmpty(country.Name) || string.IsNullOrEmpty(country.Continent) || string.IsNullOrEmpty(country.Language))
            {
                return BadRequest("Datos inválidos");
            }

            var result = _countryService.CreateCountry(country);
            if (!result)
                return BadRequest("No se pudo crear el país");

            return Ok(country);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _countryService.DeleteCountry(id);
            if (!result)
                return NotFound("No se pudo eliminar el país");
            return NoContent();
        }
    }
}