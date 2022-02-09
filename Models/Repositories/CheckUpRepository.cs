using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.Repositories
{
    public class CheckUpRepository : ICheckUpsRepository
    {
        private readonly AutoCareDBContext context;

        public CheckUpRepository(AutoCareDBContext _con)
        {
            context = _con;
        }

        public async Task<int> Add(checkUp entity)
        {
            entity.Date = DateTime.Now;
            await context.checkUps.AddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(checkUp entity)
        {
            context.checkUps.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<checkUp>> GetAll()
        {
            return await context.checkUps.Include(x=>x.car).ToListAsync();
        }

        public async Task<bool> checkID(long id)
        {
            if (await context.checkUps.AnyAsync(x => x.Id == id))
            {
                return true;
            }
            else
                return false;
        }
        public async Task<checkUp> GetById(long id)
        {
            if (checkID(id).Result)
            {
                return await context.checkUps.Where(x => x.Id == id)
                    .Include(s=>s.servicesRelation).ThenInclude(s=>s.service)
                    .Include(sp=>sp.spareRelation).ThenInclude(sp=>sp.spareParts)
                    .Include(x=>x.car).ThenInclude(x=>x.applicationUser)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<int> Update(checkUp entity)
        {
            if (await GetById(entity.Id) != null)
            {
                var oldcheck = await GetById(entity.Id);
                oldcheck.EditedDate = DateTime.Now.ToString();
                return await context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> AddServices(ServicesCheckups entity)
        {
           await context.servicesCheckups.AddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddSpares(spareCheckup entity)
        {
            await context.spareCheckups.AddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<spareCheckup>> GetSpars(long id)
        {
            return await context.spareCheckups.Where(x=>x.checkupId == id).ToListAsync();
        }
        public async Task<IEnumerable<ServicesCheckups>> GetServices(long id)
        {
            return await context.servicesCheckups.Where(x => x.checkupId == id).ToListAsync();
        }
        public async Task<int> RemoveServices(ServicesCheckups entity)
        {
             context.servicesCheckups.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> RemoveSpares(spareCheckup entity)
        {
            context.spareCheckups.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<checkUp>> GetCarChecks(long carid)
        {
            return await context.checkUps.Where(x => x.CarId == carid).OrderByDescending(x => x.Date).ToListAsync();
        }
        public async Task<IEnumerable<checkUp>> GetUserChecks(string ownerid)
        {
            var cars =await context.Cars.Where(x => x.OwnerId == ownerid).ToListAsync();
            List<checkUp> checkUpsList = new List<checkUp>();
            foreach (var item in cars)
            {
                checkUpsList.AddRange(await GetCarChecks(item.ID));
            }
            return checkUpsList;
        }
    }
    }
    
    

