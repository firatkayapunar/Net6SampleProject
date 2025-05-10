using AutoMapper;
using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Core.Models;
using Net6SampleProject.Core.Repositories;
using Net6SampleProject.Core.Services;
using Net6SampleProject.Core.UnitOfWorks;

namespace Net6SampleProject.Service.Services
{
    public class ProductService : Service<Product, ProductDto>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
            : base(productRepository, unitOfWork, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<CustomResponseDto<List<ProductsWithCategoryDto>>> GetProductsWithCategoryAsync()
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();
            var dto = _mapper.Map<List<ProductsWithCategoryDto>>(products);
            return CustomResponseDto<List<ProductsWithCategoryDto>>.Success(dto, 200);
        }
    }
}
