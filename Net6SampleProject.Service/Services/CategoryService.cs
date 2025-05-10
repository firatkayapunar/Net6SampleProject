using AutoMapper;
using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Core.Models;
using Net6SampleProject.Core.Repositories;
using Net6SampleProject.Core.Services;
using Net6SampleProject.Core.UnitOfWorks;

namespace Net6SampleProject.Service.Services
{
    public class CategoryService : Service<Category, CategoryDto>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(categoryRepository, unitOfWork, mapper)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CustomResponseDto<CategoryWithProductsDto>> GetCategoryByIdWithProductsAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdWithProductsAsync(categoryId);
            var categoryDto = _mapper.Map<CategoryWithProductsDto>(category);
            return CustomResponseDto<CategoryWithProductsDto>.Success(categoryDto, 200);
        }
    }
}
