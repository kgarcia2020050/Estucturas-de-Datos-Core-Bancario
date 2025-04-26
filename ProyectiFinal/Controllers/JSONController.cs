using Microsoft.AspNetCore.Mvc;
using Utils.JSONParser;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectiFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JSONController : ControllerBase
    {

        private JSONParser _jsonParser = new JSONParser();

        // GET: api/<JSONController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<JSONController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _jsonParser.Parse("") ? "true" : "false";
        }

        // POST api/<JSONController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<JSONController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<JSONController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
