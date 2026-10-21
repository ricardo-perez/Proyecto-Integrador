
namespace ProyectoIntegrador.Entities
{
    public enum TipoError
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
        public string Mensaje { get; }
        public TipoError TipoError { get; }
        protected Result(bool isSuccess, string mensaje, TipoError tipoError)
        {
            IsSuccess = isSuccess;
            Mensaje = mensaje ?? string.Empty;
            TipoError = tipoError;
        }
        public static Result Success(string mensaje = "Operación realizada con éxito")
        {
            return new Result(true, mensaje, TipoError.Ninguno);
        }
        public static Result Failure(string mensaje, TipoError tipoError = TipoError.Inesperado)
        {
            return new Result(false, mensaje, tipoError);
        }
    }
    public class Result<T> : Result
    {
        public T Value { get; }
        private Result(T value, bool isSuccess, string mensaje, TipoError tipoError)
            : base(isSuccess, mensaje, tipoError)
        {
            Value = value;
        }
        public static Result<T> Success(T value, string mensaje = "Operación realizada con éxito")
        {
            return new Result<T>(value, true, mensaje, TipoError.Ninguno);
        }
        public static new Result<T> Failure(string mensaje, TipoError tipoError = TipoError.Inesperado)
        {
            return new Result<T>(default, false, mensaje, tipoError);
        }
    }
}
