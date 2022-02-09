using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.Repositories
{
    public class SparePartsRepository : IAutoRepository<SpareParts>
    {
        private readonly AutoCareDBContext context;

        public SparePartsRepository(AutoCareDBContext _con)
        {
            context = _con;
        }

        public async Task<int> Add(SpareParts entity)
        {
            entity.AddedDate = DateTime.Now.ToString("G");
            await context.spareParts.AddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(SpareParts entity)
        {
             context.spareParts.Remove(entity);
            return await  context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SpareParts>> GetAll()
        {
            return await context.spareParts.ToListAsync();
        }

        public async Task<bool> checkID(long id)
        {
            if (await context.spareParts.AnyAsync(x => x.ID == id))
            {
                return true;
            }
            else
                return false;
        }
        public async Task<SpareParts> GetById(long id)
        {
            if (checkID(id).Result)
            {
                return await context.spareParts.Where(x => x.ID == id).FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<int> Update(long id, SpareParts entity)
        {
            if (await GetById(id) != null)
            {
                entity.EditedDate = DateTime.Now.ToString("G");
                var oldpart = await GetById(id);
                oldpart.Price = entity.Price;
                oldpart.Details = entity.Details;
                oldpart.EditedDate = entity.EditedDate;
                return await context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }
    }
}
