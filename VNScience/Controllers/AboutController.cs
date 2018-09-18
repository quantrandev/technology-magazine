using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Models;

namespace VNScience.Controllers
{
    public class AboutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: About
        public ActionResult Index()
        {
            var about = db.Abouts
                .Where(e => e.IsDisplayed.Value)
                .OrderByDescending(e => e.CreatedAt)
                .FirstOrDefault();

            return View(about);
        }
    }
}