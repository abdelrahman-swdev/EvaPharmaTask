using EvaPharmaTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EvaPharmaTask.Infrastructure.Data.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id, bool disableTracking = true);
        Task<IReadOnlyList<T>> ListAllAsync(bool disableTracking = true);
        IQueryable<T> FindBy(Expression<Func<T, bool>> criteria, bool disableTracking = true);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
