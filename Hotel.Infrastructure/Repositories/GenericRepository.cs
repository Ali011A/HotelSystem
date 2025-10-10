using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Models;
using Hotel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // ---------- Basic CRUD ----------

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            
            return await Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
      
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            _dbSet.RemoveRange(entities);
        }

    

    
        public virtual IQueryable<T> Query()
        {
            return _dbSet.Where(x => !x.IsDeleted);
        }

        
        public virtual IQueryable<T> QueryAsNoTracking()
        {
            return Query().AsNoTracking();
        }

        

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await Query().AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var q = Query();
            if (predicate != null) q = q.Where(predicate);
            return await q.CountAsync(cancellationToken);
        }

       
        public virtual async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            IQueryable<T> q = QueryAsNoTracking().Where(predicate);

            if (orderBy != null) q = orderBy(q);
            if (skip.HasValue) q = q.Skip(skip.Value);
            if (take.HasValue) q = q.Take(take.Value);

            return await q.ToListAsync(cancellationToken);
        }

        
        public virtual async Task<T?> SingleOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? transform = null,
            CancellationToken cancellationToken = default)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            IQueryable<T> q = Query(); // tracked by default so you can update the returned entity

            if (transform != null)
            {
                q = transform(q);
            }

            q = q.Where(predicate);

            // default: no-tracking can be applied by transform if caller wants read-only
            return await q.SingleOrDefaultAsync(cancellationToken);
        }
    }
}
