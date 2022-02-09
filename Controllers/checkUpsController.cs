using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCare.Models;
using AutoCare.Models.Repositories;
using AutoCare.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using static AutoCare.Helper;

namespace AutoCare.Controllers
{
    public class checkUpsController : Controller
    {
        private readonly ICheckUpsRepository rep;
        private readonly ICarRepository<Cars> carRepository;
        private readonly IAutoRepository<SpareParts> Srepository;
        private readonly IAutoRepository<services> servicesrepository;
        private readonly AutoCareDBContext autoCareDBContext;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public checkUpsController(ICheckUpsRepository _rep, ICarRepository<Cars> carRepository,
            IAutoRepository<SpareParts> Srepository, IAutoRepository<services> servicesrepository,
            AutoCareDBContext autoCareDBContext,
             SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            rep = _rep;
            this.carRepository = carRepository;
            this.Srepository = Srepository;
            this.servicesrepository = servicesrepository;
            this.autoCareDBContext = autoCareDBContext;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        // GET: checkUps
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            if (signInManager.IsSignedIn(User) && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
            {
                return View(await rep.GetAll());
            }
            else
            {
                return View(await rep.GetUserChecks(user.Id));
            }
        }

        [Authorize]
        [NoDirectAccess]
        public async Task<IActionResult> ExpandBill(long id)
        {
            ViewBag.id = id;
            return View(await rep.GetById(id));
        }
        [Authorize]
        [NoDirectAccess]
        public async Task<IActionResult> Details(long id)
        {

            if (await rep.GetById(id) == null)
            {
                return NotFound();
            }
            return View(await rep.GetById(id));
        }

        // GET: checkUps/Create
        [NoDirectAccess]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var spars = await Srepository.GetAll();
            var services = await servicesrepository.GetAll();

            CheckUpSpareServicesViewModel m1 = new CheckUpSpareServicesViewModel();
            m1.Services = services.Select(m => new CheckBoxServices()
            {
                Id = m.Id,
                Details = m.Details,
                price = m.price,
                priceInPoints = m.priceInPoints,
                Earnedpoints = m.Earnedpoints,
                IsChecked = false
            }).ToList();
            m1.SpareParts = spars.Select(n => new CheckBoxSpareParts()
            {
                ID = n.ID,
                Details = n.Details,
                Price = n.Price,
                Quantity=1,
                IsChecked = false
            }).ToList();
            ViewBag.cars = await carRepository.GetAll();
            return View(m1);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        [Authorize]
        
        public async Task<IActionResult> Create(CheckUpSpareServicesViewModel model, checkUp check, ServicesCheckups servicesCheckups, spareCheckup spareCheckup)
        {
            List<spareCheckup> spareCheckupsList = new List<spareCheckup>();
            List<ServicesCheckups> ServicesCheckupsList = new List<ServicesCheckups>();
            check.Counter = model.Counter;
            check.CarId = model.CarId;

            foreach (var item in model.Services)
            {
                if (item.IsChecked == true)
                {
                    var service = await servicesrepository.GetById(item.Id);
                    check.TotalCost += service.price;
                    check.TotalPoints += service.Earnedpoints;
                }

            }
            foreach (var item in model.SpareParts)
            {
                if (item.IsChecked == true)
                {
                    var spare = await Srepository.GetById(item.ID);
                    check.TotalCost += spare.Price * item.Quantity;

                }

            }

            int result = await rep.Add(check);

            if (result > 0)
            {
                long checkId = check.Id;
                foreach (var item in model.Services)
                {
                    if (item.IsChecked == true)
                    {
                        ServicesCheckupsList.Add(new ServicesCheckups() { checkupId = checkId, serviceId = item.Id });
                    }

                }
                foreach (var item in model.SpareParts)
                {
                    if (item.IsChecked == true)
                    {
                        spareCheckupsList.Add(new spareCheckup() { checkupId = checkId, spareId = item.ID,Quantity=item.Quantity });
                    }

                }

                foreach (var item in ServicesCheckupsList)
                {
                    await rep.AddServices(item);

                }
                foreach (var item in spareCheckupsList)
                {

                    await rep.AddSpares(item);
                }

            }
            return RedirectToAction("Index");


        }
      
        


        [HttpGet]
        [NoDirectAccess]
        [Authorize]
        public async Task<IActionResult> Edit(long id)
        {
            //var spars = await Srepository.GetAll();
            //var services = await servicesrepository.GetAll();
            CheckUpSpareServicesViewModel CSSVM = new CheckUpSpareServicesViewModel();
            var model = await rep.GetById(id);
            

            CSSVM.SpareParts = autoCareDBContext.spareParts.Select(n => new CheckBoxSpareParts()
            {
                ID = n.ID,
                Details = n.Details,
                Price = n.Price,
                Quantity=n.checkupRelation.Where(x => x.checkupId==id && x.spareId==n.ID).Select(x=>x.Quantity).FirstOrDefault(),
                IsChecked = n.checkupRelation.Any(x => x.checkupId == model.Id) ? true : false
            }).ToList();

            CSSVM.Services = autoCareDBContext.services.Select(m => new CheckBoxServices()
            {
                Id = m.Id,
                Details = m.Details,
                price = m.price,
                priceInPoints = m.priceInPoints,
                Earnedpoints = m.Earnedpoints,
                IsChecked = m.checkupRelation.Any(x => x.checkupId == model.Id) ? true : false
            }).ToList();
            CSSVM.CarId = model.CarId;
            CSSVM.Counter = model.Counter;
            //CSSVM.cars = autoCareDBContext.Cars.ToList();
            ViewBag.cars = await carRepository.GetAll();
            ViewBag.id = id;
            return View(CSSVM);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        [Authorize]
        public async Task<IActionResult> Edit(CheckUpSpareServicesViewModel model, long id, checkUp check, ServicesCheckups servicesCheckups, spareCheckup spareCheckup)
        {
            List<spareCheckup> spareCheckupsList = new List<spareCheckup>();
            List<ServicesCheckups> ServicesCheckupsList = new List<ServicesCheckups>();
            check.Counter = model.Counter;
            check.CarId = model.CarId;
            check.Id = id;


            foreach (var item in model.Services)
            {
                if (item.IsChecked == true)
                {
                    var service = await servicesrepository.GetById(item.Id);
                    check.TotalCost += service.price;
                    check.TotalPoints = service.Earnedpoints;
                }

            }
            foreach (var item in model.SpareParts)
            {
                if (item.IsChecked == true)
                {
                    var service = await Srepository.GetById(item.ID);
                    check.TotalCost += service.Price * item.Quantity;

                }

            }
            int result = await rep.Update(check);

            if (result > 0)
            {
                long checkId = check.Id;
                foreach (var item in model.Services)
                {
                    if (item.IsChecked == true)
                    {
                        ServicesCheckupsList.Add(new ServicesCheckups() { checkupId = checkId, serviceId = item.Id });
                    }

                }
                foreach (var item in model.SpareParts)
                {
                    if (item.IsChecked == true)
                    {
                        spareCheckupsList.Add(new spareCheckup() { checkupId = checkId, spareId = item.ID,Quantity = item.Quantity });
                    }
                }

            }

            var databasetable = await rep.GetServices(check.Id);
            var resultlist = databasetable.Except(ServicesCheckupsList).ToList();
            foreach (var item in resultlist)
            {
                await rep.RemoveServices(item);
            }
            var databasetable2 = await rep.GetSpars(check.Id);
            var resultlist2 = databasetable2.Except(spareCheckupsList).ToList();
            foreach (var item in resultlist2)
            {
                await rep.RemoveSpares(item);
            }

            var getservices = await rep.GetServices(check.Id);
            var getspars = await rep.GetSpars(check.Id);
            foreach (var item in spareCheckupsList)
            {
                if (!getspars.Contains(item))
                {
                    await rep.AddSpares(item);
                }
            }
            foreach (var item in ServicesCheckupsList)
            {
                if (!getservices.Contains(item))
                {
                    await rep.AddServices(item);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var check = await rep.GetById(id);

            await rep.Delete(check);

            return View("_ViewAll", await rep.GetAll());

        }

        public async Task<IActionResult> DetailsByCar(long carid)
        {

            if (await rep.GetCarChecks(carid) == null)
            {
                return NotFound();
            }
            ViewBag.carNumbers = carRepository.GetById(carid).Result.CarNumbers;
            return View(await rep.GetCarChecks(carid));
        }

        public async Task<IActionResult> DetailsByUser(string ownerId)
        {

            if (await rep.GetUserChecks(ownerId) == null)
            {
                return NotFound();
            }
            return View(await rep.GetUserChecks(ownerId));
        }


    }
}