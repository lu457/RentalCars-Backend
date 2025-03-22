namespace RentalCars.Domain.Exceptions
{
    public class DomainException : Exception
    {
        // Constructor que toma un mensaje de error y lo pasa a la clase base Exception
        public DomainException(string message) : base(message)
        {
        }
    }
}

