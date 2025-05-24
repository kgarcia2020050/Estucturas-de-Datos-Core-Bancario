using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Utils.JSONParser;
using Utils.models;
using ProyectiFinal.Services;
using Utils.EstructuraDeDatos.Arboles.Models;
using Utils.dtos;
using Swashbuckle.AspNetCore.Annotations;
using Utils.dtos.Requests;
using Utils.models.Requests;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectiFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly ClientService _treeService;

        public ClientController(ClientService _treeService)
        {
            this._treeService = _treeService;
        }



        [HttpGet("getAllClients")]
        public IActionResult Get()
        {
            Response response = _treeService.findAll();

            if (!response.Success)
            {
                return BadRequest(response);
            }
            response = new Response("Clientes cargados correctamente.", response.Success, response.Data, response.total);
            return Ok(response);
        }

        [HttpGet("getActiveClients")]
        public IActionResult GetActive()
        {
            Response response = _treeService.findActiveUsers();

            if (!response.Success)
            {
                return BadRequest(response);
            }
            response = new Response("Clientes activos cargados correctamente.", response.Success, response.Data, response.total );
            return Ok(response);
        }

        [HttpPost("create")]
        public IActionResult Post([FromBody] CreateUpdateClient client)
        {
            Response response = new Response();
            try
            {
                if (client == null)
                {
                    response = new Response("El cliente no puede ser nulo", false);
                    return BadRequest(response);
                }
                if (string.IsNullOrEmpty(client.dpi))
                {
                    response = new Response("El DPI no puede ser nulo o vacío", false);
                    return BadRequest(response);
                }

                // Map CreateUpdateClient to Cliente
                Cliente cliente = new Cliente
                {
                    dpi = client.dpi,
                    nombre = client.nombre,
                    apellido = client.apellido,
                    direccion = client.direccion,
                    telefono = client.telefono,
                    email = client.email,
                    genero = client.genero,
                    nacimiento = client.nacimiento,
                    nacionalidad = client.nacionalidad,
                    nit = client.nit,
                    estado = client.estado
                };

                _treeService.insert(cliente);
            }
            catch (Exception e)
            {
                response = new Response(e.Message, false);
                return BadRequest(response);
            }
            response = new Response("Cliente creado correctamente.", true, client, 1);
            return Ok(response);
        }

        // PUT api/<JSONController>/5
        [HttpPut("update")]
        public IActionResult Put([FromBody] CreateUpdateClient client)
        {
            Response response = new Response();
            try
            {
                if (client == null)
                {
                    response = new Response("El cliente no puede ser nulo", false);
                    return BadRequest(response);
                }
                if (string.IsNullOrEmpty(client.dpi))
                {
                    response = new Response("El DPI no puede ser nulo o vacío", false);
                    return BadRequest(response);
                }

                Cliente cliente = new Cliente
                {
                    dpi = client.dpi,
                    nombre = client.nombre,
                    apellido = client.apellido,
                    direccion = client.direccion,
                    telefono = client.telefono,
                    email = client.email,
                    genero = client.genero,
                    nacimiento = client.nacimiento,
                    nacionalidad = client.nacionalidad,
                    nit = client.nit,
                    estado = client.estado
                };

                _treeService.update(cliente);
            }
            catch (Exception e)
            {
                response = new Response(e.Message, false);
                return BadRequest(response);
            }
            response = new Response("Cliente actualizado correctamente.", true, client, 1);
            return Ok(response);
        }


        [HttpPut("updateStatus")]
        public IActionResult updateStatus([FromBody] ActiveInactiveRegister client)
        {
            Response response = new Response();
            object data = null;
            try
            {
                if (client == null)
                {
                    response = new Response("El cliente no puede ser nulo", false);
                    return BadRequest(response);
                }

                if (string.IsNullOrEmpty(client.dpi))
                {
                    response = new Response("El DPI no puede ser nulo o vacío", false);
                    return BadRequest(response);
                }

                Cliente cliente = new Cliente
                {
                    dpi = client.dpi,
                };
               Response statusRes =  _treeService.updateStatus(cliente);
                data = statusRes.Data;

            }
            catch (Exception e)
            {

                // Log the exception (optional)
                Debug.WriteLine($"Error: {e.Message}");
                Debug.WriteLine($"Stack Trace: {e.StackTrace}");

                response = new Response(e.Message, false);
                return BadRequest(response);
            }
            response = new Response("Cliente actualizado correctamente.", true, data, 1);
            return Ok(response);
        }

        // DELETE api/<JSONController>/5
        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string dpi)
        {

            Response response = new Response();
            {
                try
                {
                    if (dpi == null || dpi == "")
                    {
                        response = new Response("El DPI no puede ser nulo o vacio", false);
                        return BadRequest(response);
                    }

                    Cliente cliente = new Cliente();
                    cliente.dpi = dpi;

                    response = _treeService.delete(cliente);

                    if (!response.Success)
                    {
                        return BadRequest(response);
                    }

                    response = new Response("Cliente eliminado", true, response.Data, 1);
                }
                catch (Exception e)
                {
                    response = new Response(e.Message, false);
                    return BadRequest(response);
                }
                return Ok(response);
            }

        }

        [HttpPost("initialData")]
        public IActionResult uploadClients([FromBody] Clientes clientes)
        {
            Response response = new Response();
            try
                {
                _treeService.insertMany(clientes);
                }
                catch (Exception e)
                {
                    response = new Response(e.Message, false);
                    return BadRequest(response);
                }

            response = new Response("Clientes cargados correctamente.", true, null, clientes.clientes.Count);
            return Ok(response);
        }


        [HttpGet("findByDpi")]
        public IActionResult search([FromQuery] string dpi )
        {
            Response response = new Response();
            {
                try
                {
                    if (dpi == null || dpi == "")
                    {
                        response = new Response("El DPI no puede ser nulo o vacio", false);
                        return BadRequest(response);
                    }

                    Cliente cliente = new Cliente();
                    cliente.dpi = dpi;

                    response = _treeService.search(cliente);

                    if (!response.Success)
                    {
                        return BadRequest(response);
                    }

                    response = new Response("Cliente encontrado", true, response.Data, 1);
                }
                catch (Exception e)
                {
                    response = new Response(e.Message, false);
                    return BadRequest(response);
                }
                return Ok(response);
            }

        }
    }
}
