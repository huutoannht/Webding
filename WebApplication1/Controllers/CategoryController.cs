using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication2.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Entities _context;

        public CategoryController()
        {
        }

        // GET: Category
        public async Task<ActionResult> Index()
        {
            using (var _context = new Entities())
            {
                return View(_context.CaptureTypes.ToList());
            }
           
        }

        // GET: Category/Details/5
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }
            var _context = new Entities();
            var captureType = _context.CaptureTypes
                .SingleOrDefault(m => m.CaptureTypeID == id);
            if (captureType == null)
            {
                return HttpNotFound();
            }

            return View(captureType);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( CaptureType captureType)
        {
            if (ModelState.IsValid)
            {
                var _context = new Entities();
                _context.CaptureTypes.Add(captureType);
               await _context.SaveChangesAsync();
                TempData["Message"] = "create";
                return RedirectToAction(nameof(Index));
            }
            return View(captureType);
        }

        // GET: Category/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var _context = new Entities();
            var captureType = _context.CaptureTypes.SingleOrDefault(m => m.CaptureTypeID == id);
            if (captureType == null)
            {
                return HttpNotFound();
            }
            return View(captureType);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CaptureType captureType)
        {
            if (id != captureType.CaptureTypeID)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var _context = new Entities();
                    var captureTypeDb = _context.CaptureTypes.Find(id);
                    captureTypeDb = captureType;
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "update";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaptureTypeExists(captureType.CaptureTypeID))
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(captureType);
        }

        // GET: Category/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var _context = new Entities();
            var captureType = _context.CaptureTypes
                .SingleOrDefault(m => m.CaptureTypeID == id);
            if (captureType == null)
            {
                return HttpNotFound();
            }

            return View(captureType);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var _context = new Entities();
            var captureType = _context.CaptureTypes.SingleOrDefault(m => m.CaptureTypeID == id);
            captureType.IsDelete = true;
            await _context.SaveChangesAsync();
            TempData["Message"] = "delete";
            return Json(new { data = true });
        }

        private bool CaptureTypeExists(int id)
        {
            return _context.CaptureTypes.Any(e => e.CaptureTypeID == id);
        }
    }
}
