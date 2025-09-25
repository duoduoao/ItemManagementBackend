using AutoMapper;
using FluentValidation;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ItemManagement.WebAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ItemsApiController : ControllerBase
    {
        private readonly IItemsUseCases _itemsUseCases;
        private readonly IMapper _mapper;
        private readonly IValidator<ItemDto> _validator;
        private readonly ILogger<ItemsApiController> _logger;

        public ItemsApiController(
            IItemsUseCases itemsUseCases,
            IMapper mapper,
            IValidator<ItemDto> validator,
            ILogger<ItemsApiController> logger)
        {
            _itemsUseCases = itemsUseCases;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemApiModel>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var userName = User.Identity?.Name ?? "anonymous";
                _logger.LogInformation("User {UserName} accessed the protected resource", userName);

                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User id claim is missing.");
                    return Unauthorized();
                }

                var itemsDto = await _itemsUseCases.GetItemsUseCase(userId, cancellationToken);
                var itemsApiModel = _mapper.Map<IEnumerable<ItemApiModel>>(itemsDto);
                return Ok(itemsApiModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting items");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemApiModel>> Get(int id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var itemDto = await _itemsUseCases.GetItemByIdUseCase(id, userId, cancellationToken);
            if (itemDto == null) return NotFound();

            var itemApiModel = _mapper.Map<ItemApiModel>(itemDto);
            return Ok(itemApiModel);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ItemApiModel itemApiModel, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var itemDto = _mapper.Map<ItemDto>(itemApiModel);
            var validationResult = await _validator.ValidateAsync(itemDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return ValidationProblem(ModelState);
            }
            await _itemsUseCases.AddItemUseCase(itemDto, userId, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = itemDto.ItemId }, itemApiModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ItemApiModel itemApiModel, CancellationToken cancellationToken)
        {
            if (id != itemApiModel.ItemId) return BadRequest();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var itemDto = _mapper.Map<ItemDto>(itemApiModel);
            bool updated = await _itemsUseCases.EditItemUseCase(itemDto, userId, cancellationToken);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            bool deleted = await _itemsUseCases.DeleteItemUseCase(id, userId, cancellationToken);
            if (!deleted) return NotFound();

            return NoContent();
        }

        [HttpGet("category/{categoryId}")]
        public ActionResult<IEnumerable<ItemApiModel>> GetItemsByCategory(int categoryId)
        {
            var itemsDto = _itemsUseCases.ViewItemsByCategoryId(categoryId);
            if (itemsDto == null || !itemsDto.Any())
            {
                return NotFound();
            }
            var itemsApiModel = _mapper.Map<IEnumerable<ItemApiModel>>(itemsDto);
            return Ok(itemsApiModel);
        }
    }
}
