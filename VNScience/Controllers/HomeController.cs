using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Common;
using VNScience.DataAccess;
using VNScience.Models;

namespace VNScience.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ClientPostDAO postDAO;

        public HomeController()
        {
            postDAO = new ClientPostDAO(db);
        }

        public ActionResult Index()
        {
            ViewBag.TopHot = postDAO.TopHot();
            ViewBag.Recent = postDAO.Recent();
            ViewBag.MostViews = postDAO.MostViews();
            return View();
        }

        [HttpGet]
        public JsonResult Recent(int page, int pageSize)
        {
            var posts = postDAO.Recent(page, pageSize).Select(e => new
            {
                Title = e.Title,
                Author = e.CreatingUser.FullName,
                ViewCount = e.ViewCount,
                Time = DateTimeHelper.FormatDate(e.CreatedAt.Value),
                CoverImage = e.CoverImage
            });

            bool isAnyLeft = postDAO.IsAnyLeft(page, pageSize);

            return Json(new { status = 200, data = posts, isAnyLeft = isAnyLeft }, JsonRequestBehavior.AllowGet);
        }
    }
}