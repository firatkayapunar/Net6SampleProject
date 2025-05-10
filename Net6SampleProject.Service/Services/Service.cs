using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Net6SampleProject.Core.Repositories;
using Net6SampleProject.Core.Services;
using Net6SampleProject.Core.UnitOfWorks;
using Net6SampleProject.Service.Exceptions;
using System.Linq.Expressions;

namespace Net6SampleProject.Service.Services
{
    public class Service<TEntity, TDto> : IService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public Service(IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found.");
            return _mapper.Map<TDto>(entity);
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAll().ToListAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<TDto> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<TDto>(entity);
        }

        public async Task<IEnumerable<TDto>> AddRangeAsync(IEnumerable<TDto> dtos)
        {
            var entities = _mapper.Map<List<TEntity>>(dtos);
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} with ID {id} not found.");
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<int> ids)
        {
            var entities = await _repository.Where(x => ids.Contains(EF.Property<int>(x, "Id"))).ToListAsync();
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _repository.Where(expression);
        }
    }
}
