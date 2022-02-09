using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCare.Models;
using AutoCare.Models.ViewModels;
using AutoCare.Models.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using static AutoCare.Helper;

namespace AutoCare.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarRepository<Cars> rep;
        private readonly IUserRepository<ApplicationUser> rep2;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public CarsController(ICarRepository<Cars> _rep,
            IUserRepository<ApplicationUser> _rep2,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            rep = _rep;
            rep2 = _rep2;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }


        // GET: Cars
        [NoDirectAccess]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            if (signInManager.IsSignedIn(User) && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
            {
                return View(await rep.GetAll());
            }
            else
            {
                return View(await rep.GetByOwner(user.Id));
            }    

               
        }


        // GET: Cars/Details/5
        [NoDirectAccess]
        [Authorize]
        
        public async Task<IActionResult> Details(long id)
        {
            if (await rep.GetById(id) == null)
            {
                return NotFound();
            }
            
            return View(await rep.GetById(id));
        }

        // GET: Cars/Create

        [HttpGet]
        [NoDirectAccess]
        [Authorize]
        
        public async Task<IActionResult> Create()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            if (signInManager.IsSignedIn(User) && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
            {
                IEnumerable<ApplicationUser> owners = await rep2.GetAll();
                var viewmodel = new CarCreateViewModel
                {
                    owners = owners
                };
                return View(viewmodel);
            }
            else
            {
                var viewmodel = new CarCreateViewModel
                {
                    
                    ownerId = user.Id
                };
                return View(viewmodel);
            }

          
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        [Authorize]
        
        public async Task<IActionResult> Create(CarCreateViewModel car)
        {
            if (ModelState.IsValid)
            {
                var car1 = new Cars
                {                
                    CarNumbers = car.CarNumbers,
                    Model = car.Model,
                    Type = car.Type,
                    OwnerId = car.ownerId
                };

                if(await rep.checkNumbers(car.CarNumbers))
                {
                    ViewBag.numbers = "Number is already exists";
                    //return View(car);
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", car) });

                }
                var result= await rep.Add(car1);

                if (result > 0)
                {
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
                }

            }
            //return View(car);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", car) });
        }

        [HttpGet]
        [NoDirectAccess]
        [Authorize]

        // GET: Cars/Edit/5
        public async Task< IActionResult> Edit(long id)
        {
          
            var car = rep.GetById(id).Result;
            if (car == null)
            {
                return NotFound();
            }
            ApplicationUser user = await userManager.GetUserAsync(User);
            if (signInManager.IsSignedIn(User) && !User.IsInRole("User"))
            {
                var viewmodel = new CarEditViewModel
                {
                    CarNumbers = car.CarNumbers,
                    Model = car.Model,
                    Type = car.Type,
                    ownerId = car.applicationUser.Id,
                    owners = await rep2.GetAll()
                };
                ViewBag.id = id;
                return View(viewmodel);
            }
            else
            {
                var viewmodel = new CarEditViewModel
                {
                    CarNumbers = car.CarNumbers,
                    Model = car.Model,
                    Type = car.Type,
                    ownerId = car.applicationUser.Id,
                    
                };
                ViewBag.id = id;
                return View(viewmodel);
            }

            }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        [Authorize]

        public async Task<IActionResult> Edit(long id,CarEditViewModel car)
        {      
            if (ModelState.IsValid)
            {
                
                var car1 = new Cars
                {  
                    
                    CarNumbers=car.CarNumbers,
                    Model=car.Model,
                    Type=car.Type,
                    OwnerId=car.ownerId

                };
            
                var result1 =await rep.Update(id,car1);
                
                if (result1 > 0)
                {
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
                }
            }
            ViewBag.validations = "Not Valid ";

            //return View(car);

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", car) });
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        [Authorize]

        public IActionResult DeleteConfirmed(long id)
        {
            var car = rep.GetById(id).Result;
            rep.Delete(car);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", rep.GetAll().Result) });

        }


        [HttpGet]
        [NoDirectAccess]
        [Authorize]

        public async Task<IActionResult> AddCar()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            if (signInManager.IsSignedIn(User) && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
            {
                IEnumerable<ApplicationUser> owners = await rep2.GetAll();
                var viewmodel = new CarCreateViewModel
                {
                    owners = owners
                };
                return View(viewmodel);
            }
            else
            {
                var viewmodel = new CarCreateViewModel
                {

                    ownerId = user.Id
                };
                return View(viewmodel);
            }


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        [Authorize]

        public async Task<IActionResult> AddCar(CarCreateViewModel car)
        {
            if (ModelState.IsValid)
            {
                var car1 = new Cars
                {
                    CarNumbers = car.CarNumbers,
                    Model = car.Model,
                    Type = car.Type,
                    OwnerId = car.ownerId
                };

                if (await rep.checkNumbers(car.CarNumbers))
                {
                    ViewBag.numbers = "Number is already exists";
                    return View(car);

                }
                var result = await rep.Add(car1);

                if (result > 0)
                {
                    RedirectToAction("Index");        
                }

            }
            return View(car);
        }

        //public async Task<IActionResult> CarsByOwner()
        //{
        //    //var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    ApplicationUser user = await userManager.GetUserAsync(User);
        //    if (await rep.GetByOwner(user.Id) == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(await rep.GetByOwner(user.Id));
        //}
    }
}