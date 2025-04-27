using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Utils.models;

namespace Utils.JSONParser
{
    public class JSONParser
    {
        public JSONParser() { }

        public bool Parse(string json)
        {
            try
            {


                string path = Directory.GetCurrentDirectory();
                string jsonFilePath = Path.Combine(path, "JSONData", "Clientes.json");


                Clientes listaDatos;

                if (File.Exists(jsonFilePath))
                {

                    Debug.WriteLine("El archivo existe, se procederá a leerlo.");

                    string jsonExistente = File.ReadAllText(jsonFilePath);
                    Debug.WriteLine(jsonExistente);
                    listaDatos = JsonSerializer.Deserialize<Clientes>(jsonExistente);
                }
                else
                {
                    listaDatos = new Clientes();

                    Debug.WriteLine("El archivo no existe, se creará uno nuevo.");

                    return false;
                }

                Debug.WriteLine("El archivo se ha leído correctamente.");

                Cliente nuevoDato = new Cliente()
                {
                    Apellido = "Navas",
                    Nombre = "Denisse",
                    Direccion = "24 calle",
                    Telefono = "47953795",
                    Email = "marynavas170204@gmail.com",
                    Dpi = "3581244680101",
                    Estado = 1,
                    Genero = enums.Genero.F,
                    Nacimiento = "17/02/04",
                    Nacionalidad = "Guatemalteca",
                    Nit = 55555,
                    Tarjetas = new List<Tarjeta>()
                };

                // Agregar a la lista
                listaDatos.clientes.Add(nuevoDato);


                Debug.WriteLine("Se ha agregado un nuevo cliente.");

                // Configurar opciones de serialización
                var opciones = new JsonSerializerOptions
                {
                    WriteIndented = true,    // Formato legible
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                Debug.WriteLine("Se han configurado las opciones de serialización.");

                // Convertir a JSON y guardar
                string nuevoJson = JsonSerializer.Serialize(listaDatos, opciones);

                Debug.WriteLine("Se ha convertido a JSON.");

                File.WriteAllText(jsonFilePath, nuevoJson);

                Debug.WriteLine("Datos insertados correctamente!");

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error parsing JSON: {ex.Message}");
                return false;
            }


        }
    }
}