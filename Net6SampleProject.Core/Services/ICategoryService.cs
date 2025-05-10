using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Core.Models;

namespace Net6SampleProject.Core.Services
{
    public interface ICategoryService : IService<Category, CategoryDto>
    {
        Task<CustomResponseDto<CategoryWithProductsDto>> GetCategoryByIdWithProductsAsync(int categoryId);
    }
}
