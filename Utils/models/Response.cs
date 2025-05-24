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

        public Response(string message, bool success, object data, int total)
        {
            Message = message;
            Success = success;
            Data = data;
            this.total = total;
        }

        public string Message { get; set; } = string.Empty;

        public bool Success { get; set; } = false;

        public int total { get; set; } = 0;

        public object? Data { get; set; } = null;


    }
}
