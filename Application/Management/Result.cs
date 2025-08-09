namespace Comparatist.Application.Management
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Message { get; }

        public Result(bool isSuccess, string? message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result OK => new Result(true, null);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        public Result(bool isSuccess, T? value, string? message) : base(isSuccess, message)
        {
            Value = value;
        }
    }
}
