using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Web.Mvc;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using WebApplication1.Models;
using WebApplication1.Extenstions;

namespace WebApplication2.Controllers
{
    public class AlbumController : Controller
    {
        private readonly Entities _context;

        public AlbumController()
        {
            _context = new Entities();
        }

        // GET: Album
        public async Task<ActionResult> Index()
        {
            var Entities = _context.Albums.Include("CaptureType");
            return View(await Entities.ToListAsync());
        }

        // GET: Album/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var album =  _context.Albums
                .Include("CaptureTypes")
                .SingleOrDefault(m => m.AlbumID == id);
            if (album == null)
            {
                return HttpNotFound();
            }

            return View(album);
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            ViewData["CaptureTypeId"] = new SelectList(_context.CaptureTypes, "CaptureTypeId", "CaptureTypeName");
            return View();
        }

        // POST: Album/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Album album,
            HttpPostedFileBase[] fileImage)
        {
            if (ModelState.IsValid)
            {
                _context.Albums.Add(album);
                await _context.SaveChangesAsync();


                if (fileImage != null && fileImage.Length > 0)
                {
                    List<Image> listImage = new List<Image>();
                    Image image = null;
                    var uploadsImage = Path.Combine(Server.MapPath(Constants.LocaltionFile));
                    bool exists = System.IO.Directory.Exists(uploadsImage);
                    if (!exists)
                        System.IO.Directory.CreateDirectory(uploadsImage);

                    foreach (var item in fileImage)
                    {
                        image = new Image();
                        var extension = Path.GetExtension(item.FileName);
                        var fileName= Guid.NewGuid()  + extension;
                        var pathSaveFile = Path.Combine(uploadsImage, fileName);
                        item.SaveAs(pathSaveFile);

                        image.AlbumID = album.AlbumID;
                        image.ImageName = item.FileName;
                        image.ImageUrl = Constants.LocaltionFileForDisplay + fileName;
                        listImage.Add(image);
                    }
                    _context.Images.AddRange(listImage);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "create";
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaptureTypeId"] = new SelectList(_context.CaptureTypes, "CaptureTypeId", "CaptureTypeName", album.CaptureTypeID);
            return View(album);
        }

        // GET: Album/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var album = _context.Albums.Include("Images").SingleOrDefault(m => m.AlbumID == id);
            album.Images = album.Images.Where(n => !n.IsDeleted).ToList();
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewData["CaptureTypeId"] = new SelectList(_context.CaptureTypes, "CaptureTypeId", "CaptureTypeName", album.CaptureTypeID);
            return View(album);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Album album,
             HttpPostedFileBase[] fileImage)
        {
            if (id != album.AlbumID)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var albumDb = _context.Albums.Find(id);
                    albumDb = album;
                    await _context.SaveChangesAsync();

                    if (fileImage != null && fileImage.Length > 0)
                    {
                        List<Image> listImage = new List<Image>();
                        Image image = null;
                        var uploadsImage = Path.Combine(Server.MapPath( Constants.LocaltionFile));
                        bool exists = System.IO.Directory.Exists(uploadsImage);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(uploadsImage);

                        foreach (var item in fileImage)
                        {
                            image = new Image();
                            var extension = Path.GetExtension(item.FileName);
                            var fileName = Guid.NewGuid() + extension;
                            var pathSaveFile = Path.Combine(uploadsImage, fileName);
                            item.SaveAs(pathSaveFile);

                            image.AlbumID = album.AlbumID;
                            image.ImageName = item.FileName;
                            image.ImageUrl = Constants.LocaltionFileForDisplay + fileName;
                            listImage.Add(image);
                        }
                        _context.Images.AddRange(listImage);
                         _context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    if (!AlbumExists(album.AlbumID))
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        throw ex;
                    }
                }
                TempData["Message"] = "update";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaptureTypeId"] = new SelectList(_context.CaptureTypes, "CaptureTypeId", "CaptureTypeName", album.CaptureTypeID);
            return View(album);
        }

        // GET: Album/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var album =  _context.Albums
                .Include("CaptureType")
                .SingleOrDefault(m => m.AlbumID == id);
            if (album == null)
            {
                return HttpNotFound();
            }

            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var album =  _context.Albums.SingleOrDefault(m => m.AlbumID == id);
            album.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Message"] = "delete";
            return Json(new { data = true });
        }

        [HttpPost, ActionName("DeleteImage")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            var image = _context.Images.SingleOrDefault(m => m.ImageID == id);
            image.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Json(new { data = true });
        }
        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.AlbumID == id);
        }
    }
}
