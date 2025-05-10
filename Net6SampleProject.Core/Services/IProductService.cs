using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Core.Models;

namespace Net6SampleProject.Core.Services
{
    public interface IProductService : IService<Product, ProductDto>
    {
        Task<CustomResponseDto<List<ProductsWithCategoryDto>>> GetProductsWithCategoryAsync();
    }
}
