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
    public class SparePartsController : Controller
    {
        private readonly IAutoRepository<SpareParts> rep;

        public SparePartsController(IAutoRepository<SpareParts> _rep)
        {
            rep = _rep;
        }

        // GET: SpareParts
        public async Task<IActionResult> Index()
        {
            return View(await rep.GetAll());
        }

        // GET: SpareParts/Details/5
        public async Task<IActionResult> Details(long id)
        {
            if (await rep.GetById(id) == null)
            {
                return NotFound();
            }
           
            return View(await rep.GetById(id));
        }

        // GET: SpareParts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SpareParts/Create     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SparePartsViewModel spareParts)
        {
            if (ModelState.IsValid)
            {
                var spare = new SpareParts
                {
                    Details=spareParts.Details,
                    Price=spareParts.Price
                };

                var result = await rep.Add(spare);

                if(result >0)
                {
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });

                }
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", spareParts) });
        }

        // GET: SpareParts/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var part = await rep.GetById(id);
            if (part == null)
            {
                return NotFound();
            }
            var model = new SparePartsViewModel
            { 
                Details=part.Details,
                Price=part.Price           
            };
            ViewBag.id = id;
            return View(model);
        }

        // POST: SpareParts/Edit/5
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SparePartsViewModel sparePart)
        {
            if (ModelState.IsValid)
            {
                var part = new SpareParts
                {
                   Details=sparePart.Details,
                   Price=sparePart.Price
                };

                var result = await rep.Update(id, part);
                if (result > 0)
                {

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
                }
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", sparePart) });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var part = await rep.GetById(id);
            var result1 = await rep.Delete(part);
            
            if (result1 > 0)
            {
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "_ViewAll", await rep.GetAll()) });
        }


    }
}
