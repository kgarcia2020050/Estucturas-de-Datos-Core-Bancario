using System.Text.Json.Serialization;
using Utils.enums;

namespace Utils.models
{
    public class Solicitud
    {
        [JsonPropertyName("fechaSolicitud")]
        public string fechaSolicitud { get; set; }

        [JsonPropertyName("montoSolicitado")]
        public double? montoSolicitado { get; set; } 

        [JsonPropertyName("estado")]
        public string estado { get; set; }

        [JsonPropertyName("tipo")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoSolicitud tipo { get; set; }

        [JsonPropertyName("fechaRenovacion")]
        public string? fechaRenovacion { get; set; }

        [JsonPropertyName("nuevoPin")]
        public string? nuevoPin { get; set; }
    }
}
