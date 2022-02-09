using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.Repositories
{
    public class userRepository : IUserRepository<ApplicationUser>
    {
        private readonly AutoCareDBContext context;

        public userRepository(AutoCareDBContext _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            return await context.applicationUsers.OrderByDescending(x => x.AddedDate).ToListAsync();
        }
        public async Task<ApplicationUser> GetById(string id)
        {
            if ( await checkID(id))
            {
                return await context.applicationUsers.Include(a => a.cars).Where(x => x.Id == id.ToString()).FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<int> Add(ApplicationUser entity)
        {
            entity.AddedDate= DateTime.Now.ToString("dddd, dd MMMM yyyy");
            context.applicationUsers.Add(entity);
            return   await context.SaveChangesAsync();
        }

        public async Task<bool> checkID(string id)
        {
            if ( await context.applicationUsers.AnyAsync(x => x.Id == id))
            {
                return true;
            }
            else
                return false;
        }


        public void Delete(ApplicationUser entity)
        {
            var cars =  context.Cars.Where(x => x.applicationUser.Id == entity.Id).ToList();
            foreach (Cars cars1 in cars)
            {
                context.Cars.Remove(cars1);
            }
            context.applicationUsers.Remove(entity);
            context.SaveChanges();
        }

        //public async Task<int> Delete(ApplicationUser entity)
        //{
        //    var cars =await context.Cars.Where(x => x.applicationUser.Id == entity.Id).ToListAsync();
        //    foreach (Cars cars1 in cars)
        //    {
        //        context.Cars.Remove(cars1);
        //    }
        //    context.applicationUsers.Remove(entity);
        //    return await context.SaveChangesAsync();
        //}
        public async Task<int> Update(string id, ApplicationUser entity)
        {

            if (await checkID(id))
            {
                var olduser = await context.applicationUsers.FindAsync(id);
                olduser.Phone = entity.Phone;
                olduser.Email = entity.Email;
                olduser.Address = entity.Address;
                olduser.FirstName = entity.FirstName;
                olduser.LastName = entity.LastName;
                olduser.NationalNumber = entity.NationalNumber;
                olduser.EditedDate=DateTime.Now.ToString("dddd, dd MMMM yyyy");
                return await context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
           
               
        }

        public async Task<IEnumerable<Cars>> GetAllCars(string id)
        {
            if (await context.applicationUsers.AnyAsync(x => x.Id == id))
            {
                IEnumerable<Cars> cars = await context.Cars.Where(x => x.applicationUser.Id == id).ToListAsync();
                return  cars;
            }
            else
            {
                return null;
            }
        }
        
    }
}
