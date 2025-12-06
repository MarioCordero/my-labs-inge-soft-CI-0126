namespace ExamTwo.Models
{
    public enum CoffeeMachineErrorCode
    {
        None = 0,
        EmptyOrder = 1,
        OutOfStock = 2,
        InvalidPayment = 3,
        InternalError = 99
    }

    public static class CoffeeMachineErrorMessages
    {
        public const string EmptyOrder = "Orden vacía.";
        public const string OutOfStock = "No hay suficiente stock.";
        public const string InvalidPayment = "Pago inválido.";
        public const string InternalError = "Error interno del sistema.";

        public static string OutOfStockFor(string coffeeName, int available)
            => $"{OutOfStock} para '{coffeeName}'. Stock disponible: {available}.";

        public static string CoffeeNotFound(string coffeeName)
            => $"El café '{coffeeName}' no existe.";

        public static string InvalidQuantity(string coffeeName)
            => $"La cantidad solicitada para '{coffeeName}' debe ser mayor que cero.";

        public static string NotEnoughChange => "No hay suficiente cambio disponible.";
        public static string PaymentInsufficient => "Pago insuficiente.";
    }
}