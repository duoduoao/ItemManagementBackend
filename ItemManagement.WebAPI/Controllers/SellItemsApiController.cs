using AutoMapper;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellItemsApiController : ControllerBase
    {
        private readonly ISellItemUseCase _sellItemUseCase;
        private readonly IMapper _mapper;

        public SellItemsApiController(ISellItemUseCase sellItemUseCase, IMapper mapper)
        {
            _sellItemUseCase = sellItemUseCase;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SellOrderApiModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Call the use case to execute selling logic
                await _sellItemUseCase.ExecuteAsync(model.CashierName, model.ItemId, model.SellQty, cancellationToken);

                return Ok(new { message = "Sell order processed successfully." });
            }
            catch (Exception ex)
            {
                // Ideally log the exception here

                return StatusCode(500, new { error = "An error occurred while processing the sell order.", details = ex.Message });
            }
        }
    }
}
 