using Microsoft.EntityFrameworkCore;
using Net6SampleProject.Core.Models;
using Net6SampleProject.Core.Repositories;

namespace Net6SampleProject.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        { }

        public async Task<Category> GetCategoryByIdWithProductsAsync(int categoryId)
        {
            return await _dbSet
                .Include(x => x.Products)
                .SingleOrDefaultAsync(x => x.Id == categoryId);
        }
    }
}
