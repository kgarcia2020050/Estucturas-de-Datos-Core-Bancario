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
                string rutaArchivo = @"C:\Users\KennethGarcia\Documents\C#\Universidad\ProyectoFinal\ProyectiFinal\JSONData\Clientes.json";
                Clientes listaDatos;

                if (File.Exists(rutaArchivo))
                {

                    Debug.WriteLine("El archivo existe, se procederá a leerlo.");

                    string jsonExistente = File.ReadAllText(rutaArchivo);
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
                    Apellido = "Apellido",
                    Nombre = "Nombre",
                    Direccion = "Direccion",
                    Telefono = "Telefono",
                    Email = "Email",
                    Dpi = "DPI",
                    Estado = 1,
                    Genero = enums.Genero.M,
                    Nacimiento = "Nacimiento",
                    Nacionalidad = "Nacionalidad",
                    Nit = 11111,
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

                File.WriteAllText(rutaArchivo, nuevoJson);

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