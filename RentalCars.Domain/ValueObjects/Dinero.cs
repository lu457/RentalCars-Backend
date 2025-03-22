namespace RentalCars.Domain.ValueObjects
{
    public record Dinero
    {
        public decimal Monto { get; }  // Monto del dinero
        public string Moneda { get; } = "USD";  // Moneda, con valor predeterminado en "USD"

        public Dinero(decimal monto, string moneda = "USD")
        {
            Monto = monto;
            Moneda = moneda;
        }
    }
}

