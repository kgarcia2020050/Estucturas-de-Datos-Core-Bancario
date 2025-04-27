using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;
using Utils.models;

namespace ProyectiFinal.Services
{
    public class JSONService
    {
        Clientes listaDatos;


        public JSONService()
        {
        }



        public bool loadJsonData()
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                string jsonFilePath = Path.Combine(path, "JSONData", "Clientes.json");

                if (File.Exists(jsonFilePath))
                {

                    Debug.WriteLine("El archivo existe, se procederá a leerlo.");

                    string jsonExistente = File.ReadAllText(jsonFilePath);
                    Debug.WriteLine(jsonExistente);
                    listaDatos = JsonSerializer.Deserialize<Clientes>(jsonExistente);
                    return true;
                }
                else
                {

                    Debug.WriteLine("El archivo no existe, se creará uno nuevo.");

                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al cargar el archivo JSON: " + ex.Message);
                return false;
            }
        }


        public bool clientExistsByDpi(string dpi, string excludeDpi = null)
        {


            if (!loadJsonData())
            {
                Debug.WriteLine("No hay clientes en la lista.");
                return false;
            }

            foreach (Cliente client in listaDatos.clientes)
            {

                if (excludeDpi != null && client.Dpi == excludeDpi)
                    continue;

                if (client.Dpi == dpi)
                {
                    Debug.WriteLine("Ya existe un cliente con el DPI " + dpi);
                    return false;
                }
            }

            return true;
        }

        public bool clientExistsByNit(string nit, string excludeDpi = null)
        {
            if (!loadJsonData())
            {
                Debug.WriteLine("No hay clientes en la lista.");
                return false;
            }

            foreach (Cliente client in listaDatos.clientes)
            {

                if (excludeDpi != null && client.Dpi == excludeDpi)
                    continue;

                if (client.Nit.ToString() == nit)
                {
                    Debug.WriteLine("Ya existe un cliente con el NIT " + nit);
                    return false;
                }
            }

            return true;
        }

        public bool clientExistsByEmail(string email, string excludeDpi = null)
        {
            if (!loadJsonData())
            {
                Debug.WriteLine("No hay clientes en la lista.");
                return false;
            }

            foreach (Cliente client in listaDatos.clientes)
            {

                if (excludeDpi != null && client.Dpi == excludeDpi)
                    continue;

                if (client.Email == email)
                {
                    Debug.WriteLine("Ya existe un cliente con el correo " + email);
                    return false;
                }
            }

            return true;

        }

        public bool clientExistsByPhone(string phone, string excludeDpi = null)
        {
            if (!loadJsonData())
            {
                Debug.WriteLine("No hay clientes en la lista.");
                return false;
            }
            foreach (Cliente client in listaDatos.clientes)
            {
                if (excludeDpi != null && client.Dpi == excludeDpi)
                    continue;

                if (client.Telefono == phone)
                {
                    Debug.WriteLine("Ya existe un cliente con el teléfono " + phone);
                    return false;
                }
            }
            return true;
        }


        public List<Cliente> getClients()
        {
            if (!loadJsonData())
            {
                Debug.WriteLine("No hay clientes en la lista.");
                return null;
            }
            return listaDatos.clientes;
        }


        public Response addClient(Cliente client)
        {

            Response response;
            if (!loadJsonData())
            {
                Debug.WriteLine("No hay clientes en la lista.");

                response = new Response("No hay clientes en la lista.", false);
            }
            else
            {
                if (clientExistsByDpi(client.Dpi) && clientExistsByNit(client.Nit.ToString()) && clientExistsByEmail(client.Email) && clientExistsByPhone(client.Telefono))
                {
                    listaDatos.clientes.Add(client);
                    string json = JsonSerializer.Serialize(listaDatos, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "Clientes.json"), json);
                    Debug.WriteLine("Cliente agregado correctamente.");
                    response = new Response("Cliente agregado correctamente.", true);
                }
                else
                {
                    Debug.WriteLine("No se pudo crear el cliente, debido a que ya existe uno con la misma información ingresada (Revisar DPI, NIT, Email y Teléfono)");
                    response = new Response("No se pudo crear el cliente, debido a que ya existe uno con la misma información ingresada (Revisar DPI, NIT, Email y Teléfono)", false);
                }

            }

            return response;


        }

        public Response updateClient(Cliente client)
        {
            Response response = null;

            if (!loadJsonData())
            {
                Debug.WriteLine("No hay clientes en la lista.");
                response = new Response("No hay clientes en la lista.", false);
            }


            else
            {
                Clientes copyClients = listaDatos;

                foreach (Cliente c in copyClients.clientes)
                {
                    if (c.Dpi == client.Dpi)
                    {
                        bool existingClient =
                            !clientExistsByDpi(client.Dpi, c.Dpi) ||
                            !clientExistsByNit(client.Nit.ToString(), c.Dpi) ||
                            !clientExistsByEmail(client.Email, c.Dpi) ||
                            !clientExistsByPhone(client.Telefono, c.Dpi);

                        if (existingClient)
                        {
                            Debug.WriteLine("No se pudo actualizar el cliente, debido a que ya existe uno con la misma información ingresada (Revisar DPI, NIT, Email y Teléfono)");
                            response = new Response("No se pudo actualizar el cliente, debido a que ya existe uno con la misma información ingresada (Revisar DPI, NIT, Email y Teléfono)", false);
                            break;
                        }

                        c.Nombre = client.Nombre;
                        c.Apellido = client.Apellido;
                        c.Direccion = client.Direccion;
                        c.Telefono = client.Telefono;
                        c.Email = client.Email;
                        c.Genero = client.Genero;
                        c.Nacimiento = client.Nacimiento;
                        c.Nacionalidad = client.Nacionalidad;
                        c.Nit = client.Nit;
                        c.Estado = client.Estado;
                        c.Tarjetas = client.Tarjetas;

                        Debug.WriteLine("Cliente: " + c.ToString() + " actualizado correctamente.");

                        string json = JsonSerializer.Serialize(copyClients, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "Clientes.json"), json);
                        response = new Response("Cliente actualizado correctamente.", true);
                        break;
                    }
                }

                if (response == null)
                {
                    Debug.WriteLine("El cliente no existe.");
                    response = new Response("El cliente solicitado no existe.", false);
                }
            }

            return response;
        }


        public Response deleteClient(string dpi)
        {

            Response response = null;

            if (!loadJsonData())
            {
                Debug.WriteLine("No hay clientes en la lista.");
                response = new Response("No hay clientes en la lista.", false);
            }
            else
            {
                foreach (Cliente c in listaDatos.clientes)
                {
                    if (c.Dpi == dpi)
                    {
                        listaDatos.clientes.Remove(c);
                        string json = JsonSerializer.Serialize(listaDatos, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "JSONData", "Clientes.json"), json);
                        Debug.WriteLine("Cliente eliminado correctamente.");
                        response = new Response("Cliente eliminado correctamente.", true);
                        break;
                    }
                }

            }
            if (response == null)
            {
                Debug.WriteLine("El cliente no existe.");
                response = new Response("El cliente solicitado no existe.", false);
            }

            return response;


        }

    }
}
