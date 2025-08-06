using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Interfaces;
using System.Linq.Expressions;

namespace RealState.Infraestructure.Persistence.Repositories
{
    public class BaseRepository<TEntity, TKey> : IRepositoryBase<TEntity, TKey> where TEntity : class
    {
        protected readonly RealStateContext _context;
        protected readonly DbSet<TEntity> _entitySet;

        protected BaseRepository(RealStateContext context)
        {
            _context = context;
            _entitySet = _context.Set<TEntity>();
        }

        public async virtual Task<Result<TEntity>> AddAsync(TEntity entity)
        {
            try
            {
                _entitySet.Add(entity);
                await _context.SaveChangesAsync();
                return Result<TEntity>.Ok(entity);
            }
            catch (Exception ex)
            {
                return Result<TEntity>.Fail(ex.Message);
            }
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
            => await _entitySet.AnyAsync(predicate);

        public IQueryable<TEntity> AsQuery()
            => _entitySet.AsQueryable();

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
            => await _entitySet.CountAsync(predicate);

        public async virtual Task<Result<Unit>> DeleteAsync(TEntity entity)
        {
            try
            {

                _entitySet.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<Unit>.Ok(Unit.Value);
            }
            catch (Exception ex)
            {
                return Result<Unit>.Fail(ex.Message);
            }
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
            => await _entitySet.FirstOrDefaultAsync(predicate);



        public async Task<IEnumerable<TEntity>> GetAllAsync()
             => await _entitySet.ToListAsync();

        public async Task<Result<TEntity>> GetById(TKey id)
        {
            var entity = await _entitySet.FindAsync(id);
            if (entity == null)
            {
                return Result<TEntity>.Fail($"Entity with id {id} not found.");
            }
            return Result<TEntity>.Ok(entity);
        }

        public virtual async Task<PaginatedResponse<TEntity>> GetPaginated(Pagination pagination)
        {
            var data = await _context.Set<TEntity>()
                .Skip(pagination.PageSize * (pagination.PageNumber - 1))
                .Take(pagination.PageSize).ToListAsync();
            return new PaginatedResponse<TEntity>
            {
                Data = data,
                Pagination = pagination
            };
        }

        public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                _entitySet.Update(entity);
                await _context.SaveChangesAsync();
                return Result<TEntity>.Ok(entity);
            }
            catch (Exception ex)
            {
                return Result<TEntity>.Fail(ex.Message);
            }
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
            => await _entitySet.Where(predicate).ToListAsync();

        public virtual async Task<List<TEntity>> GetAllListWithInclude(List<string> properties)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync(); //EF - immediate execution
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
