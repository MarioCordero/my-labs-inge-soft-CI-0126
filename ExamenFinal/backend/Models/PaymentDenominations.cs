namespace ExamTwo.Models
{
    public class PaymentDenominations
    {
        public Dictionary<int, int> Coins { get; set; }  // { 1000: 5, 500: 10, 100: 20, 50: 30, 25: 15 }
        public Dictionary<int, int> Bills { get; set; }   // { 1000: 3, 500: 2 }
    }
}