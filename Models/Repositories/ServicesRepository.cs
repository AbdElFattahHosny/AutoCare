using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.Repositories
{
    public class ServicesRepository : IAutoRepository<services>
    {
        private readonly AutoCareDBContext context;

        public ServicesRepository(AutoCareDBContext _con)
        {
            context = _con;
        }

        public async Task<int> Add(services entity)
        {
            entity.AddedDate = DateTime.Now.ToString("G");
            await context.services.AddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(services entity)
        {
            context.services.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<services>> GetAll()
        {
            return await context.services.ToListAsync();
        }

        public async Task<bool> checkID(long id)
        {
            if (await context.services.AnyAsync(x => x.Id == id))
            {
                return true;
            }
            else
                return false;
        }
        public async Task<services> GetById(long id)
        {
            if (checkID(id).Result)
            {
                return await context.services.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<int> Update(long id, services entity)
        {
            if (await GetById(id) != null)
            {
                entity.EditedDate = DateTime.Now.ToString("G");
                var oldservice = await GetById(id);
                oldservice.Details = entity.Details;
                oldservice.price = entity.price;
                oldservice.priceInPoints = entity.priceInPoints;
                oldservice.Earnedpoints = entity.Earnedpoints;
                oldservice.EditedDate = entity.EditedDate;
                return await context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }
    }
}

