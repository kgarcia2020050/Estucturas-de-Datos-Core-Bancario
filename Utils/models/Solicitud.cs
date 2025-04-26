using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils.enums;

namespace Utils.models
{
    public class Solicitud
    {
        [JsonPropertyName("fechaSolicitud")]
        public string FechaSolicitud { get; set; }

        [JsonPropertyName("montoSolicitado")]
        public double? MontoSolicitado { get; set; } 

        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        [JsonPropertyName("tipo")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoSolicitud Tipo { get; set; }

        [JsonPropertyName("fechaRenovacion")]
        public string? FechaRenovacion { get; set; }
    }
}
