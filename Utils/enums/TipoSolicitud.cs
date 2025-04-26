using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Utils.enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoSolicitud
    {
        [JsonPropertyName("AUMENTO")]
        Aumento,

        [JsonPropertyName("DESBLOQUEO")]
        Desbloqueo,

        [JsonPropertyName("RENOVACION")]
        Renovacion
    }
}
