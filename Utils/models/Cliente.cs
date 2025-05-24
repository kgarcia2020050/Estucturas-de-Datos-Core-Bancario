using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils.enums;
using Utils.EstructuraDeDatos.Arboles.Models;
using Utils.EstructuraDeDatos.ListasEnlazadas;
using Utils.EstructuraDeDatos.ListasEnlazadas.Models;

namespace Utils.models
{
    public class Cliente
    {

        public Cliente()
        {
            tarjetasLista = new ListaEnlazada();
        }


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

        [JsonPropertyName("estado")]
        public int estado { get; set; }

        [JsonPropertyName("tarjetas")]
        public List<Tarjeta>? tarjetas { get; set; }

        [JsonIgnore] 
        public ListaEnlazada tarjetasLista { get; set; }



        public bool addTarjeta(Tarjeta tarjeta)
        {
            if (tarjetasLista == null)
            {
                tarjetasLista = new ListaEnlazada();
            }
            tarjetasLista.Insertar(tarjeta);
            return true;
        }



        public Tarjeta returnTarjeta(string numeroTarjeta, string pin)
        {
            if (tarjetasLista == null)
            {
                return null;
            }

            Tarjeta tarjeta = new Tarjeta();
            tarjeta.numeroTarjeta = numeroTarjeta;
            tarjeta.pin = pin;

            NodoLista nodo = tarjetasLista.searchItem(tarjeta);

            if (nodo != null)
            {
                return (Tarjeta)nodo.getItem();
            }
            else
            {
                return null;
            }
        }
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
            sb.AppendLine($"Tarjetas: {tarjetas}");
            return sb.ToString();
        }


    }
}
