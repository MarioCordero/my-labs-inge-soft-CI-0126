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
    }
}