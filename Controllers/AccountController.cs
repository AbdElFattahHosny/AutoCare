using AutoCare.Models;
using AutoCare.Models.Repositories;
using AutoCare.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserRepository<ApplicationUser> rep;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IUserRepository<ApplicationUser> _rep, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            rep = _rep;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {

            return View(rep.GetAll().Result);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var user = new ApplicationUser
                {
                    UserName = model.FirstName + " " + model.LastName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    NationalNumber = model.NationalNumber,
                    Phone = model.password,
                    Email=model.Email,
                    AddedDate = DateTime.Now.ToString()
                };

                if ((rep.GetAll().Result.Any(x => x.NationalNumber == user.NationalNumber)))
                {
                    ViewBag.NationalNumber = "National Number is already exist , please write different one";
                    return View(model);

                }
                var result = await userManager.CreateAsync(user, model.password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    ViewBag.user = user.UserName;
                    return RedirectToAction("AddCar","Cars");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return View("Register");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result1 = rep.GetAll().Result;
                var result = result1.ToList().Where(x => x.NationalNumber == model.NationalNumber && x.Phone == model.password).FirstOrDefault();
                if (result != null)
                {
                    await signInManager.SignInAsync(result, isPersistent: false);
                    //var roles = await userManager.GetRolesAsync(result);
                    //if (roles.Contains("Admin"))
                    //{
                        return RedirectToAction("Index", "Cars");
                    
                  
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        // GET: Cars/Details/5
        public IActionResult Details(string id)
        {
            var result = rep.GetAllCars(id).Result;
            ViewBag.Numberofcars = result.Count();
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return View(rep.GetById(id).Result);
            }

        }
        // GET: Cars/Edit/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {

            var user =  await rep.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            var viewmodel = new EditUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Email=user.Email,
                NationalNumber = user.NationalNumber,
                Address = user.Address
            };
            ViewBag.id = id;
            return View(viewmodel);
        }

      

    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit(string id,EditUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var user1 = new ApplicationUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.Email,
                    Phone = user.Phone,
                    NationalNumber=user.NationalNumber
                };

                var result = await rep.Update(id,user1);
                if (result > 0)
                {

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
                }
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", user) });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> DeleteConfirmed(string id)
        {
            var user = await rep.GetById(id);

             rep.Delete(user);
             return View("_ViewAll", await rep.GetAll());
            //return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });

            //var user = await rep.GetById(id);
            //    var result1 = await rep.Delete(user);
            //    var result2 = await rep.GetAll();
            //    if (result1 > 0)
            //    {
            //        return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", result2) });
            //    }
            //   return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "_ViewAll", result2) });
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.FirstName + " " + model.LastName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Email=model.Email,
                    NationalNumber = model.NationalNumber,
                    Phone = model.password,
                    AddedDate = DateTime.Now.ToString()
                };
                if ((rep.GetAll().Result.Any(x => x.NationalNumber == user.NationalNumber)))
                {
                    ViewBag.NationalNumber = "National Number is already exist , please write different one";
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddUser", model) });
                }
                var result1 = await rep.Add(user);
                
               await userManager.AddToRoleAsync(user, "user");
                
                
                if (result1 > 0)
                {               
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
                }    
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddUser", model) });

        }
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult>
        ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public async Task<IActionResult> EditByName(string name)
        //{

        //}

            //    var user = await rep.GetById(id);
            //    if (user == null)
            //    {
            //        return NotFound();
            //    }
            //    var viewmodel = new EditUserViewModel
            //    {
            //        FirstName = user.FirstName,
            //        LastName = user.LastName,
            //        Phone = user.Phone,
            //        Email = user.Email,
            //        NationalNumber = user.NationalNumber,
            //        Address = user.Address
            //    };
            //    ViewBag.id = id;
            //    return View(viewmodel);
            //}

        }
}

