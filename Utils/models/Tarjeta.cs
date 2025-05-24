using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils.enums;
using Utils.EstructuraDeDatos.ListasEnlazadas.Colas;
using Utils.EstructuraDeDatos.ListasEnlazadas.Pilas;

namespace Utils.models
{
    public class Tarjeta
    {

        public Tarjeta()
        {
            transacciones = new Pila();
            colaSolicitudes = new Cola();
        }

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
        public int estado { get; set; }

        [JsonPropertyName("tipo")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTarjeta tipo { get; set; }

        [JsonPropertyName("historialTransacciones")]
        public List<Transaccion>? historialTransacciones { get; set; }

        [JsonIgnore]
        public Pila? transacciones { get; set; }


        [JsonIgnore]
        public Cola? colaSolicitudes { get; set; }


        [JsonPropertyName("solicitudes")]
        public List<Solicitud>? solicitudes { get; set; }


        public Tarjeta clone()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.numeroTarjeta = this.numeroTarjeta;
            tarjeta.fechaExpiracion = this.fechaExpiracion;
            tarjeta.cvv = this.cvv;
            tarjeta.pin = this.pin;
            tarjeta.limiteCredito = this.limiteCredito;
            tarjeta.saldoActual = this.saldoActual;
            tarjeta.estado = this.estado;
            tarjeta.tipo = this.tipo;
            tarjeta.historialTransacciones = this.historialTransacciones;
            tarjeta.transacciones = this.transacciones;
            tarjeta.colaSolicitudes = this.colaSolicitudes;
            return tarjeta;
        }
    }

}
