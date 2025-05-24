using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils.enums;
using Utils.EstructuraDeDatos.ListasEnlazadas.Colas;
using Utils.EstructuraDeDatos.ListasEnlazadas.Pilas;
using Utils.models;

namespace Utils.dtos.Requests
{
    public class CreateUpdateCards
    {

        [JsonPropertyName("dpi")]
        public string dpi { get; set; }

        [JsonPropertyName("numeroTarjeta")]
        public string numeroTarjeta { get; set; }

        [JsonPropertyName("fechaExpiracion")]
        public string fechaExpiracion { get; set; }

        [JsonPropertyName("cvv")]
        public string cvv { get; set; }

        [JsonPropertyName("pin")]
        public string pin { get; set; }

        [JsonPropertyName("limiteCredito")]
        public double limiteCredito { get; set; }

        [JsonPropertyName("saldoActual")]
        public double saldoActual { get; set; }

        [JsonPropertyName("estado")]
        [JsonIgnore]
        public int estado { get; set; }

        [JsonPropertyName("tipo")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTarjeta tipo { get; set; }

    }
}
