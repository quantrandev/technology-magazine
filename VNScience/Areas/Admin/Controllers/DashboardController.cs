using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VNScience.Common;
using VNScience.Models;

namespace VNScience.Areas.Admin.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            var posts = db.Posts.ToList();

            foreach (var post in posts)
            {
                post.ViewCount = 0;
            }

            db.SaveChanges();

            return View();
        }
    }
}