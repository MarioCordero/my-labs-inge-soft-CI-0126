// ICoffeeService.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using ExamTwo.Models;

namespace ExamTwo.Services
{
    public interface ICoffeeService
    {
        Task<IEnumerable<Coffee>> GetCoffeeOptionsAsync();
        // Task<ChangeResult> ProcessPurchaseAsync(OrderRequest request);
    }
}