namespace Mubbi.Marketplace.API.Models
{

    public class ApiResponse
    {
        public ApiResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; protected set; }
        public string Message { get; protected set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(bool success, string message, T data)
            : base(success, message)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public T Data { get; private set; }
    }
}
