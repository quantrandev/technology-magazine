using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Common;
using VNScience.Models;

namespace VNScience.Areas.Admin.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        PostDAO postDAO;

        public DashboardController()
        {
            postDAO = new PostDAO(db);
        }

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.RequestsToDeleteCount = postDAO.GetRequestsToDeleteCount();
            ViewBag.RequestsToApproveCount = postDAO.GetRequestsToApproveCount();
            ViewBag.UserCount = db.Users.Count();
            ViewBag.PostsCount = postDAO.GetPostsCount();
            ViewBag.PostsByUser = postDAO.GetPostCountByUser(User.Identity.GetUserId());
            ViewBag.MostViews = postDAO.MostViews(1, 10);
            return View();
        }

        [HttpGet]
        public JsonResult MostViews(int page, int pageSize)
        {
            var posts = postDAO.MostViews(page, pageSize).Select(e => new
            {
                Title = e.Title,
                Author = e.CreatingUser.FullName,
                ViewCount = e.ViewCount,
                Time = DateTimeHelper.FormatDate(e.CreatedAt.Value),
                CoverImage = e.CoverImage
            }); ;

            return Json(new { status = 200, data = posts }, JsonRequestBehavior.AllowGet);
        }
    }
}