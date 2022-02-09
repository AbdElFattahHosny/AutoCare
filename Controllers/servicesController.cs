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

namespace AutoCare.Controllers
{
    public class servicesController : Controller
    {
        private readonly IAutoRepository<services> rep;

        public servicesController(IAutoRepository<services> _rep)
        {
            rep = _rep;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            return View(await rep.GetAll());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(long id)
        {
            if (await rep.GetById(id) == null)
            {
                return NotFound();
            }

            return View(await rep.GetById(id));
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicesViewModel service)
        {
            if (ModelState.IsValid)
            {
                var serve = new services
                {
                    Details = service.Details,
                    price = service.price,
                    priceInPoints=service.priceInPoints,
                    Earnedpoints=service.Earnedpoints

                };

                var result = await rep.Add(serve);

                if (result > 0)
                {
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });

                }
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", service) });
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var serve = await rep.GetById(id);
            if (serve == null)
            {
                return NotFound();
            }
            var model = new ServicesViewModel
            {
                Details = serve.Details,
                price = serve.price,
                priceInPoints=serve.priceInPoints,
                Earnedpoints=serve.Earnedpoints
            };
            ViewBag.id = id;
            return View(model);
        }

        // POST: Services/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServicesViewModel serve)
        {
            if (ModelState.IsValid)
            {
                var serve1 = new services
                {
                    Details = serve.Details,
                    price = serve.price,
                    priceInPoints = serve.priceInPoints,
                    Earnedpoints = serve.Earnedpoints
                };

                var result = await rep.Update(id, serve1);
                if (result > 0)
                {

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
                }
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", serve) });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var service = await rep.GetById(id);
            var result1 = await rep.Delete(service);

            if (result1 > 0)
            {
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
        }


    }
}
