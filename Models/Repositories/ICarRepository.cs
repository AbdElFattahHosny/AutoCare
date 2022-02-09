using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models
{
    public  interface ICarRepository<TEntity>
    {
         Task<IEnumerable<TEntity>> GetAll();
         Task<TEntity> GetById(long id);

        Task<int> Add(TEntity entity);
        Task<int> Update(long id, TEntity entity);
       
        void Delete(TEntity entity);

         Task<TEntity> GetByNumbers(long numbers);

         Task<IEnumerable<TEntity>> GetByOwner(string ownerId);

         Task<bool> checkID(long id);

         Task<bool> checkNumbers(long numbers);

        //public Task<IEnumerable<TEntity>> Getcars(long ownerId);
    }
}
