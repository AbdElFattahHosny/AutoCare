using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.Repositories
{
   public interface IAutoRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(long id);

        Task<int> Add(TEntity entity);
        Task<int> Update(long id, TEntity entity);

        Task<int> Delete(TEntity entity);

        Task<bool> checkID(long id);
    }
}
