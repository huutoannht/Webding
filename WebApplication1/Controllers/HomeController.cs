using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            return HttpNotFound();
        }
        [OutputCache(Duration = 5)]
        public ActionResult Category()
        {
            using (var _context = new Entities())
            {
                var listAlbum = _context.Albums.Include("Images").ToList();
                return PartialView("Category", listAlbum);
            };

        }
    }
}
