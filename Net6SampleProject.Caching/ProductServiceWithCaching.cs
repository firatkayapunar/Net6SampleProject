using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Core.Models;
using Net6SampleProject.Core.Repositories;
using Net6SampleProject.Core.Services;
using Net6SampleProject.Core.UnitOfWorks;
using Net6SampleProject.Service.Exceptions;
using System.Linq.Expressions;

namespace Net6SampleProject.Service.Services
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productsCache";

        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public ProductServiceWithCaching(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;

            if (!_memoryCache.TryGetValue(CacheProductKey, out _))
                RefreshCacheAsync().Wait();
        }

        private async Task RefreshCacheAsync()
        {
            var products = await _productRepository.GetAll().ToListAsync();
            _memoryCache.Set(CacheProductKey, products);
        }

        private List<Product> GetCachedProducts()
        {
            if (_memoryCache.TryGetValue(CacheProductKey, out List<Product> products))
                return products;

            throw new Exception("Product cache is unexpectedly empty.");
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = GetCachedProducts().FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new NotFoundException($"{nameof(Product)} with ID {id} not found.");
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = GetCachedProducts();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> AddAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();
            await RefreshCacheAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> AddRangeAsync(IEnumerable<ProductDto> productDtos)
        {
            var entities = _mapper.Map<List<Product>>(productDtos);
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await RefreshCacheAsync();
            return _mapper.Map<List<ProductDto>>(entities);
        }

        public async Task UpdateAsync(ProductDto productDto)
        {
            var entity = _mapper.Map<Product>(productDto);
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await RefreshCacheAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await _productRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"{nameof(Product)} with ID {id} not found.");

            _productRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await RefreshCacheAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<int> ids)
        {
            var products = await _productRepository.Where(p => ids.Contains(p.Id)).ToListAsync();
            _productRepository.RemoveRange(products);
            await _unitOfWork.CommitAsync();
            await RefreshCacheAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            return await _productRepository.AnyAsync(expression);
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _productRepository.Where(expression);
        }

        public async Task<CustomResponseDto<List<ProductsWithCategoryDto>>> GetProductsWithCategoryAsync()
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();
            var dto = _mapper.Map<List<ProductsWithCategoryDto>>(products);
            return CustomResponseDto<List<ProductsWithCategoryDto>>.Success(dto, 200);
        }
    }
}
