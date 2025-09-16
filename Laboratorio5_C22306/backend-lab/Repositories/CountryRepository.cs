using backend_lab_C22306.Models;
using Dapper;
using Microsoft.Data.SqlClient;

/// <summary>
/// Repository for accessing country data from the SQL Server database.
/// </summary>
namespace backend_lab_C22306.Repositories
{
    /// <summary>
    /// Handles database operations related to countries.
    /// </summary>
    public class CountryRepository
    {
        /// <summary>
        /// Connection string for the SQL Server database.
        /// </summary>
        private readonly string? _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// Loads the connection string from configuration.
        /// </summary>
        public CountryRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("CountryContext");
        }

        /// <summary>
        /// Retrieves all countries from the database.
        /// </summary>
        /// <returns>List of <see cref="CountryModel"/> objects.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the connection string is not found.</exception>
        public List<CountryModel> GetCountries()
        {
            if (_connectionString == null)
                throw new InvalidOperationException("Connection string not found.");

            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM dbo.Country";
            return connection.Query<CountryModel>(query).ToList();
        }
        public bool CreateCountry(CountryModel country)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sql = "INSERT INTO Country (Name, Continent, Language) VALUES (@Name, @Continent, @Language)";
                    var result = connection.Execute(sql, country);
                    return result > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteCountry(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Country WHERE Id = @Id";
                var rows = connection.Execute(sql, new { Id = id });
                return rows > 0;
            }
        }
    }
}