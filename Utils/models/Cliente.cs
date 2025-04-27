using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils.enums;

namespace Utils.models
{
    public class Cliente
    {
        [JsonPropertyName("dpi")]
        public string Dpi { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("apellido")]
        public string Apellido { get; set; }

        [JsonPropertyName("direccion")]
        public string Direccion { get; set; }

        [JsonPropertyName("telefono")]
        public string Telefono { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("genero")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Genero Genero { get; set; }

        [JsonPropertyName("nacimiento")]
        public string Nacimiento { get; set; }

        [JsonPropertyName("nacionalidad")]
        public string Nacionalidad { get; set; }

        [JsonPropertyName("nit")]
        public int Nit { get; set; }

        [JsonPropertyName("estado")]
        public int Estado { get; set; }

        [JsonPropertyName("tarjetas")]
        public List<Tarjeta>? Tarjetas { get; set; }


        public override string ToString()
        {
            return $"DPI: {Dpi}, Nombre: {Nombre}, Apellido: {Apellido}, Direccion: {Direccion}, Telefono: {Telefono}, Email: {Email}, Genero: {Genero}, Nacimiento: {Nacimiento}, Nacionalidad: {Nacionalidad}, Nit: {Nit}, Estado: {Estado}";
        }


    }
}
