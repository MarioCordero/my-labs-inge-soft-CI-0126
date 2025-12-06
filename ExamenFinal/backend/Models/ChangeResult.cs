namespace ExamTwo.Models
{
    public class ChangeResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public int ChangeAmount { get; set; } // Cash to return to the customer
        public Dictionary<int, int> ChangeBreakdown { get; set; }  // Key: Value of the coin, Value: Quantity of that coin
        // Create the string message for the change breakdown
        public string GetFormattedChangeMessage() 
        {
            if (!IsSuccess) 
                return ErrorMessage;
            string breakdown = string.Join(", ", 
                ChangeBreakdown.OrderByDescending(c => c.Key)
                               .Select(c => $"{c.Value} moneda de {c.Key}"));
                               
            return $"Su vuelto es de: {ChangeAmount} colones. Desglose: {breakdown}";
        }
    }
}