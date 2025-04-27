using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils.enums;

namespace Utils.models
{
    public class Tarjeta
    {
        [JsonPropertyName("numeroTarjeta")]
        public string NumeroTarjeta { get; set; }

        [JsonPropertyName("fechaExpiracion")]
        public string FechaExpiracion { get; set; }

        [JsonPropertyName("cvv")]
        public string Cvv { get; set; }

        [JsonPropertyName("pin")]
        public string Pin { get; set; }

        [JsonPropertyName("limiteCredito")]
        public double LimiteCredito { get; set; }

        [JsonPropertyName("saldoActual")]
        public double SaldoActual { get; set; }

        [JsonPropertyName("estado")]
        public int Estado { get; set; }

        [JsonPropertyName("tipo")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTarjeta Tipo { get; set; }

        [JsonPropertyName("historialTransacciones")]
        public List<Transaccion>? HistorialTransacciones { get; set; }

        [JsonPropertyName("solicitudes")]
        public List<Solicitud>? Solicitudes { get; set; }
    }

}
