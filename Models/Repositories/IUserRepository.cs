using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.Repositories
{
   public interface IUserRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        
        Task<TEntity> GetById(string id);
        Task<int> Add(TEntity entity);

        Task<int> Update(string id, TEntity entity);
        void Delete(TEntity entity);

        Task<bool> checkID(string id);

        Task<IEnumerable<Cars>> GetAllCars(string id);






    }
}
