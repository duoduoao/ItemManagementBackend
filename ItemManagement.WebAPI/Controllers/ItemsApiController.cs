using AutoMapper;
using FluentValidation;
using ItemManagement.Application.Common.DTO;
 
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
        public ItemsApiController(IItemsUseCases itemsUseCases, IMapper mapper, IValidator<ItemDto> validator, ILogger<ItemsApiController> logger)
        {
            _itemsUseCases = itemsUseCases;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ItemApiModel>> Get()
        {

            var userName = User.Identity?.Name ?? "anonymous";
            _logger.LogInformation("User {UserName} accessed the protected resource", userName);
            var itemsDto =   _itemsUseCases.GetItemsUseCase();
            var itemsApiModel = _mapper.Map<IEnumerable<ItemApiModel>>(itemsDto);
            return Ok(itemsApiModel);
        }

        [HttpGet("{id}")]
        public async Task< ActionResult<ItemApiModel>> Get(int id)
        {
            var itemDto = await _itemsUseCases.GetItemByIdUseCase(id);
            if (itemDto == null) return NotFound();

            var itemApiModel = _mapper.Map<ItemApiModel>(itemDto);
            return Ok(itemApiModel);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ItemApiModel itemApiModel)
        {
            var itemDto =   _mapper.Map<ItemDto>(itemApiModel);
            // Perform FluentValidation manually
            var validationResult = await _validator.ValidateAsync(itemDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return ValidationProblem(ModelState);
            }
            await _itemsUseCases.AddItemUseCase(itemDto);
            return CreatedAtAction(nameof(Get), new { id = itemDto.ItemId }, itemApiModel);


        }

        [HttpPut("{id}")]
        public async Task< ActionResult> Update(int id, ItemApiModel itemApiModel)
        {
            if (id != itemApiModel.ItemId) return BadRequest();

            var itemDto = _mapper.Map<ItemDto>(itemApiModel);
            bool updated = await _itemsUseCases.EditItemUseCase(itemDto);

            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool deleted = await _itemsUseCases.DeleteItemUseCase(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
        // GET api/items/category/5
        [HttpGet("category/{categoryId}")]
        public  ActionResult<IEnumerable<ItemApiModel>> GetItemsByCategory(int categoryId)
        {
            var itemsDto =   _itemsUseCases.ViewItemsByCategoryId(categoryId);

            if (itemsDto == null || !itemsDto.Any())
            {
                return NotFound();
            }

            var itemsApiModel = _mapper.Map<IEnumerable<ItemApiModel>>(itemsDto);
            return Ok(itemsApiModel);
        }
    }
}
