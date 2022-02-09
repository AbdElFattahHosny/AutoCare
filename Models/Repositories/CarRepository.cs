using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.Repositories
{
    public class CarRepository : ICarRepository<Cars>
    {
        private readonly AutoCareDBContext context;

        public CarRepository(AutoCareDBContext _context)
        {
            context = _context;
        }
        public  async Task<int> Add(Cars entity)
        {
              entity.AddedDate=DateTime.Now.ToString("G");
              await context.Cars.AddAsync(entity);
             return await context.SaveChangesAsync();
        }

     

        public  void Delete(Cars car)
        {
             context.Cars.Remove(car);
             context.SaveChanges();
        }

        public async Task<IEnumerable<Cars>> GetAll()
        {
            return  await context.Cars.Include(x=>x.applicationUser).ToListAsync();
        }

        public  async Task<int> Update(long id,Cars entity)
        {
            if(await GetById(id)!=null)
            {
                entity.EditedDate = DateTime.Now.ToString("G");
                var oldcar = await GetById(id);
                oldcar.CarNumbers = entity.CarNumbers;
                oldcar.Model = entity.Model;
                oldcar.Type = entity.Type;
                oldcar.OwnerId = entity.OwnerId;
                return await context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
      
            
           
        }  


        public async Task<bool> checkID(long id)
        {
            if (await context.Cars.AnyAsync(x => x.ID == id))
            {
                return  true;
            }
            else
               return  false;
        }
        public async Task<Cars> GetById(long id)
        {
           if(checkID(id).Result)
            {
                return await context.Cars.Include(x=>x.applicationUser).Where(x => x.ID == id).FirstOrDefaultAsync();
            }
           else
            {
                return null;
            }
        }
        public async Task<bool> checkNumbers(long numbers)
        {
            if (await context.Cars.AnyAsync(x => x.CarNumbers == numbers))
            {
                return true;
            }
            else
                return false;
        }
        public async Task<Cars> GetByNumbers(long numbers)
        {
            if (checkNumbers(numbers).Result)
            {
                return await context.Cars.Where(x => x.CarNumbers == numbers).FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Cars>> GetByOwner(string id)
        {
           if(await context.applicationUsers.AnyAsync(x=>x.Id==id))
            {
                return await context.Cars.Include(x => x.applicationUser).Where(x => x.applicationUser.Id == id).ToListAsync();
            }
           else
            {
                return null;
            }
        }

   

    }
}
