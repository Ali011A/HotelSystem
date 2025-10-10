using Hotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        //Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        //Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        //Task AddAsync(T entity, CancellationToken cancellationToken = default);
        //void Update(T entity);
        //void Delete(T entity);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);

      //عشان استخدامها في كل مكان وكمان استخدامها في كل مكان
        IQueryable<T> Query();

        // عشان هنا    تبقي 
        IQueryable<T> QueryAsNoTracking();

        // Useful helpers (async)
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

        // 
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
                                       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                       int? skip = null,
                                       int? take = null,
                                       CancellationToken cancellationToken = default);

        
        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                      Func<IQueryable<T>, IQueryable<T>>? include = null,
                                      CancellationToken cancellationToken = default);

     
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void RemoveRange(IEnumerable<T> entities);
    }
}
