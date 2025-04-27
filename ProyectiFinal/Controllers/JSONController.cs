using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Utils.JSONParser;
using Utils.models;
using ProyectiFinal.Services;
using Utils.models.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectiFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JSONController : ControllerBase
    {


        private JSONService _jsonService = new JSONService();

        // GET: api/<JSONController>
        [HttpGet]
        public IActionResult Get()
        {

            Response response;
            if (!_jsonService.loadJsonData())
            {
                response = new Response("No hay clientes en la lista.", false);
                return BadRequest(response);
            }

            List<Cliente> clientes = _jsonService.getClients();

            response = new Response("Clientes cargados correctamente.", true, clientes);
            return Ok(response);

        }

        // GET api/<JSONController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return _jsonParser.Parse("") ? "true" : "false";
        //}

        // POST api/<JSONController>
        [HttpPost]
        public IActionResult Post([FromBody] Cliente client)
        {
            Response response = _jsonService.addClient(client);
            return Ok(response);
        }

        // PUT api/<JSONController>/5
        [HttpPut]
        public IActionResult Put([FromBody] Cliente client)
        {
            Response response = _jsonService.updateClient(client);
            return Ok(response);
        }

        // DELETE api/<JSONController>/5
        [HttpDelete]
        public IActionResult Delete([FromBody] Requests dpi)
        {

            Response response = _jsonService.deleteClient(dpi.Dpi);
            return Ok(response);

        }
    }
}
