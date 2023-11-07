using EvaPharmaTask.Core.Entities;
using System;
using System.Threading.Tasks;

namespace EvaPharmaTask.Infrastructure.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
    }
}
