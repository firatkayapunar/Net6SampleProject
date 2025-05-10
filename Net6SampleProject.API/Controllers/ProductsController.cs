using Microsoft.AspNetCore.Mvc;
using Net6SampleProject.API.Filters;
using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Core.Models;
using Net6SampleProject.Core.Services;

namespace Net6SampleProject.API.Controllers
{
    public class ProductsController : CustomBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _productService.GetAllAsync();
            return CreateActionResult(CustomResponseDto<IEnumerable<ProductDto>>.Success(data, 200));
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Product, ProductDto>))]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _productService.GetByIdAsync(id);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(data, 200));
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            var created = await _productService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                CustomResponseDto<ProductDto>.Success(created, 201));
        }

        // PUT: api/products
        [HttpPut]
        [ServiceFilter(typeof(NotFoundFilter<Product, ProductDto>))]
        public async Task<IActionResult> Update(ProductDto dto)
        {
            await _productService.UpdateAsync(dto);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(NotFoundFilter<Product, ProductDto>))]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.RemoveAsync(id);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        // GET: api/products/getproductswithcategory
        [HttpGet("getproductswithcategory")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            var result = await _productService.GetProductsWithCategoryAsync();
            return CreateActionResult(result);
        }
    }
}
