using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Utils.enums
{
    public enum TipoTarjeta
    {
        [JsonPropertyName("DEBITO")]
        Debito,

        [JsonPropertyName("CREDITO")]
        Credito
    }
}
