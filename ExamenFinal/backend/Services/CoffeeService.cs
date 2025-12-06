// Services/CoffeeService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}