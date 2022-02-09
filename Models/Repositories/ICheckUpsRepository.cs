using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Models.Repositories
{
    public interface ICheckUpsRepository
    {
        Task<IEnumerable<checkUp>> GetAll();
        Task<checkUp> GetById(long id);
        Task<IEnumerable<spareCheckup>> GetSpars(long id);
        Task<IEnumerable<ServicesCheckups>> GetServices(long id);
        Task<int> Add(checkUp entity);
        Task<int> Update(checkUp entity);

        Task<int> Delete(checkUp entity);

        Task<bool> checkID(long id);

        Task<IEnumerable<checkUp>> GetCarChecks(long carid);
        Task<IEnumerable<checkUp>> GetUserChecks(string ownerid);
        Task<int> AddServices(ServicesCheckups entity);
        Task<int> AddSpares(spareCheckup entity);
        Task<int> RemoveServices(ServicesCheckups entity);
        Task<int> RemoveSpares(spareCheckup entity);

    }
}
