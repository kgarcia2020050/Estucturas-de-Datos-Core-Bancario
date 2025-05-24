using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.dtos
{
    public class ValidationResponse
    {
        public string message { get; set; }
        public bool success { get; set; }

        public ValidationResponse(string message, bool success)
        {
            this.message = message;
            this.success = success;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Message: {message}");
            sb.AppendLine($"Success: {success}");
            return sb.ToString();
        }

    }
}
