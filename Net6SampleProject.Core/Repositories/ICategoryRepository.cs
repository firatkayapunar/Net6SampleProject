using Net6SampleProject.Core.Models;

namespace Net6SampleProject.Core.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetCategoryByIdWithProductsAsync(int categoryId);
    }
}
