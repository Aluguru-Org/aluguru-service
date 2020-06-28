using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.API.Models
{
    public class ApiResponse
    {
        public ApiResponse(bool success, string message)
        {
            Success = success;
            Message = message;
            Data = null;
        }

        public ApiResponse(bool success, string message, object data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public bool Success { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }
    }
}
