using Microsoft.AspNetCore.Mvc;
using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Core.Services;

namespace Net6SampleProject.API.Controllers
{
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return CreateActionResult(CustomResponseDto<IEnumerable<CategoryDto>>.Success(result, 200));
        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return CreateActionResult(CustomResponseDto<CategoryDto>.Success(result, 200));
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            var result = await _categoryService.AddAsync(categoryDto);
            return CreateActionResult(CustomResponseDto<CategoryDto>.Success(result, 201));
        }

        // PUT: api/categories
        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto categoryDto)
        {
            await _categoryService.UpdateAsync(categoryDto);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.RemoveAsync(id);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        // GET: api/categories/GetCategoryWithProducts/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategoryWithProducts(int id)
        {
            var result = await _categoryService.GetCategoryByIdWithProductsAsync(id);
            return CreateActionResult(result);
        }
    }
}
