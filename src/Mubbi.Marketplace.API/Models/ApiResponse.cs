using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.API.Models
{
    public class ApiResponse
    {
        public ApiResponse(string message = "The request finished successfully")
        {
            Success = true;
            Message = message;
            Data = null;
            Errors = new List<string>();
        }
        public ApiResponse(object data, string message = "The request finished successfully")
        {
            Success = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
        }

        public ApiResponse(List<string> errors, string message = "The server was unable to process the request.")
        {
            Success = false;
            Message = message;
            Errors = errors;
        }
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }
        public List<string> Errors { get; private set; }
    }
}
