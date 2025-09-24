using AutoMapper;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SellOrderApiModel model)
        {
            // Call the use case to execute selling logic
            await _sellItemUseCase.ExecuteAsync(model.CashierName, model.ItemId, model.SellQty);

            return Ok(new { message = "Sell order processed successfully." });
        }

    }

}
