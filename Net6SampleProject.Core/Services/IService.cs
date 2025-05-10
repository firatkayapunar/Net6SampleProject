using System.Linq.Expressions;

namespace Net6SampleProject.Core.Services
{
    public interface IService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        Task<TDto> GetByIdAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> AddAsync(TDto dto);
        Task<IEnumerable<TDto>> AddRangeAsync(IEnumerable<TDto> dtos);
        Task UpdateAsync(TDto dto);
        Task RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<int> ids);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
    }
}
