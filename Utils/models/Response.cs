using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.models
{
    public class Response
    {

        public Response() { }

        public Response(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public Response(string message, bool success, object data)
        {
            Message = message;
            Success = success;
            Data = data;
        }

        public string Message { get; set; } = string.Empty;

        public bool Success { get; set; } = false;

        public object? Data { get; set; } = null;

    }
}
