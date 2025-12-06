namespace ExamTwo.Models
{
    public class OrderRequest
    {
        public Dictionary<string, int> Order { get; set; } // Example: { "Espresso": 2, "Latte": 1 } 
        public int TotalPayment { get; set; } 
        public PaymentDetails Payment { get; set; } 
    }
    
    public class PaymentDetails
    {
        public List<int> Coins { get; set; }
        public List<int> Bills { get; set; }
    }
}