namespace RentalCars.Application.Common
{
    public record Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }

        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        // Método para indicar éxito
        public static Result<T> Success(T value) => new(true, value, null);

        // Método para indicar fallo
        public static Result<T> Failure(string error) => new(false, default, error);

        // Método que permite manejar el éxito o el fallo de manera específica
        public TResult Match<TResult>(
            Func<T, TResult> success,
            Func<string, TResult> failure)
            => IsSuccess ? success(Value!) : failure(Error!);
    }
}

