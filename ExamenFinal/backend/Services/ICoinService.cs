using ExamTwo.Models;

namespace ExamTwo.Services
{
    public interface ICoinService
    {
        Task<PaymentDenominations> GetPaymentDenominationsAsync();
    }
}