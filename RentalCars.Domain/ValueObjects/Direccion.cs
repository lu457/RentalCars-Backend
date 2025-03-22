namespace RentalCars.Domain.ValueObjects
{
    public record Direccion
    {
        public string Calle { get; }
        public string Ciudad { get; }
        public string Pais { get; }

        public Direccion(string calle, string ciudad, string pais)
        {
            Calle = calle;
            Ciudad = ciudad;
            Pais = pais;
        }
    }
}

