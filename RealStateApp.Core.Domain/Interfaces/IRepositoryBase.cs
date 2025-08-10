using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Base;
using System.Linq.Expressions;

namespace RealStateApp.Core.Domain.Interfaces
{
    public interface IRepositoryBase<TEntity, TId> where TEntity : class
    {
        Task<Result<TEntity>> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        //Task<IEnumerable<TEntity>> GetActivesAsync();
        Task<Result<TEntity>> GetById(TId id);
        Task<Result<TEntity>> UpdateAsync(int id, TEntity entity);
        Task<Result<TEntity>> UpdateAsync(TEntity entity);
        Task<Result<Unit>> DeleteAsync(TEntity entity);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> AsQuery();
        Task<PaginatedResponse<TEntity>> GetPaginated(Pagination pagination);

        Task DeleteAsync(int id);
        Task<List<TEntity>> GetAllListWithInclude(List<string> properties);

    }
}
