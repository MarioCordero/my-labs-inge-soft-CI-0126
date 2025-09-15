using backend_lab_C22306.Models;
using backend_lab_C22306.Repositories;

namespace backend_lab_C22306.Services
{
    /// <summary>
    /// Service layer for country-related business logic.
    /// </summary>
    public class CountryService
    {
        /// <summary>
        /// Repository for accessing country data.
        /// </summary>
        private readonly CountryRepository countryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryService"/> class.
        /// </summary>
        public CountryService()
        {
            countryRepository = new CountryRepository();
        }

        /// <summary>
        /// Retrieves all countries using the repository.
        /// </summary>
        /// <returns>List of <see cref="CountryModel"/> objects.</returns>
        public List<CountryModel> GetCountries()
        {
            return countryRepository.GetCountries();
        }

        public string CreateCountry(CountryModel country)
        {
            try
            {
                bool success = countryRepository.CreateCountry(country);
                return success ? "" : "No se pudo crear el país";
            }catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}