using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Utils.models
{
    public class Transaccion
    {
        [JsonPropertyName("fecha")]
        public string fecha { get; set; }

        [JsonPropertyName("hora")]
        public string hora { get; set; }

        [JsonPropertyName("monto")]
        public double monto { get; set; }

        [JsonPropertyName("tipo")]
        public string tipo { get; set; }

        [JsonPropertyName("descripcion")]
        public string descripcion { get; set; }

        [JsonPropertyName("autorizada")]
        public bool autorizada { get; set; }
    }

}
