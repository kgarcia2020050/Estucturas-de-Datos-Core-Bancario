using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils.models.Requests;

namespace Utils.models
{
    public class Clientes
    {

        [JsonConstructor]
        public Clientes(List<Cliente> clientes)
        {
            this.clientes = clientes ?? new List<Cliente>();
        }
        public Clientes()
        {
            this.clientes = new List<Cliente>();
        }

        [JsonPropertyName("clientes")]
        public List<Cliente> clientes { get; set; }
    }
}
