namespace MovieApp.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public ErrorType Error { get; }
        private Result(T value) { IsSuccess = true; Value = value; }
        private Result(ErrorType error) { IsSuccess = false; Error = error; }
        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(ErrorType error) => new Result<T>(error);
    }
    public enum ErrorType { None, NotFound, Validation, Unexpected }
}
