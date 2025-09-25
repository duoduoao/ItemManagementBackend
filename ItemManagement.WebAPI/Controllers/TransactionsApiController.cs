using AutoMapper;
using Microsoft.AspNetCore.Mvc; 
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.WebAPI.Models; 

namespace ItemManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsApiController : ControllerBase
    {
        private readonly ITransactionsUseCases _transactionsUseCases;
        private readonly IMapper _mapper;
        public TransactionsApiController(ITransactionsUseCases transactionsUseCases, IMapper mapper)
        {
            _transactionsUseCases = transactionsUseCases;
            _mapper = mapper;
        }
        // POST api/transactions

        [HttpPost("record")]
        public IActionResult Record([FromBody] SellOrderApiModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //no dto mapping needed here
            _transactionsUseCases.RecordTransactionAsync(request.CashierName, request.ItemId, request.SellQty);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<IEnumerable<TransactionApiModel>> GetTransactions()
        {

            var transactions =  _transactionsUseCases.GetAllTransactions();

            var transactionsApiModel = _mapper.Map<IEnumerable<TransactionApiModel>>(transactions); // Use your IMapper instance

            return Ok(transactionsApiModel);
        }
    }
}
