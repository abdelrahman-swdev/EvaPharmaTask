using EvaPharmaTask.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EvaPharmaTask.Infrastructure.Data.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;

        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> criteria, bool disableTracking = true)
        {
            return disableTracking
                ? _context.Set<T>().Where(criteria).AsNoTracking()
                : _context.Set<T>().Where(criteria);
        }

        public async Task<T> GetByIdAsync(int id, bool disableTracking = true)
        {
            return disableTracking
                ? await _context.Set<T>().Where(a => a.Id == id).AsNoTracking().FirstOrDefaultAsync()
                : await _context.Set<T>().Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(bool disableTracking = true)
        {
            return disableTracking 
                ? await _context.Set<T>().AsNoTracking().ToListAsync() 
                : await _context.Set<T>().ToListAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
