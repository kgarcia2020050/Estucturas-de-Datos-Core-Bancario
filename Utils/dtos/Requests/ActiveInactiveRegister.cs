using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Utils.dtos.Requests
{
    public class ActiveInactiveRegister
    {

        [JsonPropertyName("dpi")]
        public string dpi { get; set; }


    }
}
