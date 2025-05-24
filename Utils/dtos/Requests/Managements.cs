using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Utils.dtos.Requests
{
    public class Managements
    {

        [JsonPropertyName("dpi")]
        public string dpi { get; set; }

        [JsonPropertyName("numeroTarjeta")]
        public string numeroTarjeta { get; set; }

        [JsonPropertyName("pin")]
        public string pin { get; set; }
    }
}
