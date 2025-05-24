using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils.enums;
using Utils.EstructuraDeDatos.ListasEnlazadas;

namespace Utils.models.Requests
{
    public class CreateUpdateClient
    {

        [JsonPropertyName("dpi")]
        public string dpi { get; set; }

        [JsonPropertyName("nombre")]
        public string nombre { get; set; }

        [JsonPropertyName("apellido")]
        public string apellido { get; set; }

        [JsonPropertyName("direccion")]
        public string direccion { get; set; }

        [JsonPropertyName("telefono")]
        public string telefono { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("genero")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Genero genero { get; set; }

        [JsonPropertyName("nacimiento")]
        public string nacimiento { get; set; }

        [JsonPropertyName("nacionalidad")]
        public string nacionalidad { get; set; }

        [JsonPropertyName("nit")]
        public int nit { get; set; }

        [JsonIgnore]
        [JsonPropertyName("estado")]
        public int estado { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"DPI: {dpi}");
            sb.AppendLine($"Nombre: {nombre}");
            sb.AppendLine($"Apellido: {apellido}");
            sb.AppendLine($"Direccion: {direccion}");
            sb.AppendLine($"Telefono: {telefono}");
            sb.AppendLine($"Email: {email}");
            sb.AppendLine($"Genero: {genero}");

            sb.AppendLine($"Nacimiento: {nacimiento}");
            sb.AppendLine($"Nacionalidad: {nacionalidad}");
            sb.AppendLine($"Nit: {nit}");
            sb.AppendLine($"Estado: {estado}");
            return sb.ToString();
        }


    }
}
