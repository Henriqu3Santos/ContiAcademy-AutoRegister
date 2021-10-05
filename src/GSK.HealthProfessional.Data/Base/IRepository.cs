using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GSK.HealthProfessional.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();        
        void Remove(Guid id);

        IEnumerable<TEntity> Query();

        IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter);
        
    }
}
