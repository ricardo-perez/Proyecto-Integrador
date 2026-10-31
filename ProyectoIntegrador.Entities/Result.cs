
namespace ProyectoIntegrador.Entities
{
    public enum ErrorType
    {
        Ninguno = 0,
        Validacion = 1,
        NoEncontrado = 2,
        Conflicto = 3,
        BaseDeDatos = 4,
        Inesperado = 5
    }
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Message { get; }
        public ErrorType ErrorType { get; }
        protected Result(bool isSuccess, string mensaje, ErrorType tipoError)
        {
            IsSuccess = isSuccess;
            Message = mensaje ?? string.Empty;
            ErrorType = tipoError;
        }
        public static Result Success(string mensaje = "Operación realizada con éxito")
        {
            return new Result(true, mensaje, ErrorType.Ninguno);
        }
        public static Result Failure(string mensaje, ErrorType tipoError = ErrorType.Inesperado)
        {
            return new Result(false, mensaje, tipoError);
        }
    }
    public class Result<T> : Result
    {
        public T Value { get; }
        private Result(T value, bool isSuccess, string mensaje, ErrorType tipoError)
            : base(isSuccess, mensaje, tipoError)
        {
            Value = value;
        }
        public static Result<T> Success(T value, string mensaje = "Operación realizada con éxito")
        {
            return new Result<T>(value, true, mensaje, ErrorType.Ninguno);
        }
        public static new Result<T> Failure(string mensaje, ErrorType tipoError = ErrorType.Inesperado)
        {
            return new Result<T>(default, false, mensaje, tipoError);
        }
    }
}
