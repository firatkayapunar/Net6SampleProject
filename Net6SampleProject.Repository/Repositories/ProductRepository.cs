using Microsoft.EntityFrameworkCore;
using Net6SampleProject.Core.Models;
using Net6SampleProject.Core.Repositories;

namespace Net6SampleProject.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        { }

        public async Task<List<Product>> GetProductsWithCategoryAsync()
        {
            return await _dbSet.Include(p => p.Category).ToListAsync();
        }
    }
}
