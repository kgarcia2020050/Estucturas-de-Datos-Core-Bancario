using Microsoft.AspNetCore.Mvc;
using ProyectiFinal.Services;
using Utils.dtos.Requests;
using Utils.models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectiFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {

        private readonly CardService cardService;
        private readonly ClientService _treeService;

        public CardsController(ClientService _treeService)
        {
            this._treeService = _treeService;
            this.cardService = new CardService(_treeService.bst, _treeService.avl, _treeService.cardsAvl, _treeService);
        }

        // POST api/<CardsController>
        [HttpPost("addCard")]
        public IActionResult Post([FromBody] CreateUpdateCards value)
        {
            try
            {
                Response response = cardService.addClientCard(value);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                response = new Response("Tarjeta agregada correctamente.", response.Success, response.Data, response.total);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));

            }

        }

        [HttpPost("retrieveBalances")]
        public IActionResult retrieveBalances(RetrieveBalances retrieveBalances)
        {
            try
            {
                Response response = cardService.retrieveBalances(retrieveBalances);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                response = new Response("Consulta de saldo exitosa.", response.Success, response.Data, response.total);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }


        [HttpPost("retrieveTransactions")]
        public IActionResult retrieveTransactions(RetrieveBalances retrieveTransactions)
        {
            try
            {
                Response response = cardService.retrieveTransactions(retrieveTransactions);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                response = new Response("Consulta de transacciones exitosa.", response.Success, response.Data, response.total);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }

        [HttpPost("makePayments")]
        public IActionResult makePayments([FromBody] MakePayments makePayments)
        {
            try
            {
                Response response = cardService.makePayments(makePayments);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                response = new Response(response.Message, response.Success, response.Data, response.total);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }


        [HttpPut("updatePin")]
        public IActionResult updatePin([FromBody] Managements updatePin)
        {
            try
            {
                Response response = cardService.updatePin(updatePin);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                response = new Response(response.Message, response.Success, response.Data, response.total);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }

        [HttpPut("updateStatus")]
        public IActionResult updateStatus([FromBody] Managements updateStatus)
        {
            try
            {
                Response response = cardService.blockCard(updateStatus);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                response = new Response(response.Message, response.Success, response.Data, response.total);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }

        [HttpPut("updateExpirationDate")]
        public IActionResult updateExpirationDate([FromBody] Managements updateExpirationDate)
        {
            try
            {
                Response response = cardService.renovateExpirationDate(updateExpirationDate);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                response = new Response(response.Message, response.Success, response.Data, response.total);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }

        [HttpPut("incrementLimit")]
        public IActionResult incrementLimit([FromBody] Managements incrementLimit)
        {
            try
            {
                Response response = cardService.incrementCreditLimit(incrementLimit);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                response = new Response(response.Message, response.Success, response.Data, response.total);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }
    }
}
