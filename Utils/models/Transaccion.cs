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
        public string Fecha { get; set; }

        [JsonPropertyName("hora")]
        public string Hora { get; set; }

        [JsonPropertyName("monto")]
        public double Monto { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("autorizada")]
        public bool Autorizada { get; set; }
    }

}
