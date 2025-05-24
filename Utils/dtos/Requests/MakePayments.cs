using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Utils.dtos.Requests
{
   public class MakePayments
    {

        [JsonPropertyName("dpi")]
        public string dpi { get; set; }

        [JsonPropertyName("numeroTarjeta")]
        public string numeroTarjeta { get; set; }

        [JsonPropertyName("pin")]
        public string pin { get; set; }

        [JsonPropertyName("monto")]
        public double monto { get; set; }

        [JsonPropertyName("tipo")]
        public string tipo { get; set; }

        [JsonPropertyName("descripcion")]
        public string descripcion { get; set; }
        [JsonPropertyName("cvv")]
        public string cvv { get; set; }
        [JsonPropertyName("fechaExpiracion")]

        public string fechaExpiracion { get; set; }

    }
}
