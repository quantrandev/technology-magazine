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
            return View();
        }
    }
}