using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
  
using ItemManagement.Application.UseCases;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.WebAPI.Models;
using ItemManagement.Application.Common;
using ItemManagement.Application.Common.DTO;

namespace ItemManagement.WebAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoriesUseCases _categoriesUseCases;
        private readonly IMapper _mapper;

        public CategoriesApiController(
            ICategoriesUseCases categoriesUseCases,
            IMapper mapper)
        {
            _categoriesUseCases = categoriesUseCases;
            _mapper = mapper;
        }

        // GET api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryApiModel>>> Get()
        {
            var categoriesDto = await _categoriesUseCases.GetCategoriesUseCase();
            var categoriesApiModel = _mapper.Map<IEnumerable<CategoryApiModel>>(categoriesDto);
            return Ok(categoriesApiModel);
        }

        // GET api/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryApiModel>> Get(int id)
        {
            var categoryDto = await _categoriesUseCases.GetCategoryByIdUseCase(id);
            if (categoryDto == null) return NotFound();
            var categoryApiModel = _mapper.Map<CategoryApiModel>(categoryDto);
            return Ok(categoryApiModel);
        }

        // POST api/categories
        [HttpPost]
        public ActionResult Add(CategoryApiModel categoryApiModel)
        {
            var categoryDto = _mapper.Map<CategoryDto>(categoryApiModel);
            _categoriesUseCases.AddCategoryUseCase(categoryDto);
            return CreatedAtAction(nameof(Get), new { id = categoryDto.CategoryId }, categoryApiModel);
        }

        // PUT api/categories/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CategoryApiModel categoryApiModel)
        {
            if (id != categoryApiModel.CategoryId) return BadRequest();
            var categoryDto = _mapper.Map<CategoryDto>(categoryApiModel);
            bool updated =await  _categoriesUseCases.EditCategoryUseCase(categoryDto);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE api/categories/{id}
        [HttpDelete]
        public  async Task <ActionResult> Delete(CategoryApiModel categoryApiModel)
        {
            //bool deleted = _categoriesUseCases.DeleteCategoryUseCase(id);
            var categoryDto = _mapper.Map<CategoryDto>(categoryApiModel);
         
            // Call your use case with the DTO
            var result = await _categoriesUseCases.DeleteCategoryUseCase(categoryDto);
            //var result = await _categoriesUseCases.DeleteCategoryUseCase(id);

            switch (result)
            {
                case DeleteCategoryResult.Success:
                    return NoContent();

                case DeleteCategoryResult.NotFound:
                    return NotFound();
                case DeleteCategoryResult.HasLinkedItems:
                    return Conflict(new
                    {
                        ErrorCode = "LinkedItemsConflict",
                        Message = "Cannot delete category because it has linked items."
                    });

                case DeleteCategoryResult.UnknownError:
                default:
                    return StatusCode(500, "Unknown error occurred.");
            }
        }
    }
}
