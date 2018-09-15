using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Common;
using VNScience.Models;

namespace VNScience.Areas.Admin.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        PostDAO postDAO;
        PostCategoryDAO postCategoryDAO;
        SystemInfoDAO systemInfoDAO;

        public CommonController()
        {
            postDAO = new PostDAO(db);
            postCategoryDAO = new PostCategoryDAO(db);
            systemInfoDAO = new SystemInfoDAO(db);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebar()
        {
            ViewBag.PostRequestsToApproveCount = postDAO.GetRequestsToApproveCount();
            ViewBag.PostRequestsToDeleteCount = postDAO.GetRequestsToDeleteCount();
            ViewBag.PostCategoryRequestsToDeleteCount = postCategoryDAO.GetRequestsToDeleteCount();
            ViewBag.Brand = systemInfoDAO.GetBrand();
            return PartialView("Sidebar");
        }

        [ChildActionOnly]
        public PartialViewResult Navigation()
        {
            return PartialView("Navigation");
        }

        [ChildActionOnly]
        public string Avatar()
        {
            var avatar = db.Users.Find(User.Identity.GetUserId()).Avatar;

            return string.IsNullOrEmpty(avatar) ? Common.Constants.AvatarPlaceholderUrl : avatar;
        }

        [ChildActionOnly]
        public string FullName()
        {
            var fullName = db.Users.Find(User.Identity.GetUserId()).FullName;
            return fullName;
        }

        public PartialViewResult Flash()
        {
            if (Session["Flashes"] == null)
                return PartialView("Flash", new List<Flash>());

            IEnumerable<Flash> flashes = (IEnumerable<Flash>)Session["Flashes"];
            //empty flashes
            Session["Flashes"] = new List<Flash>();
            return PartialView("Flash", flashes);
        }

        public PartialViewResult MultiSelect(MySelectList list)
        {
            return PartialView(list);
        }

        public PartialViewResult Select(MySelectList list)
        {
            return PartialView(list);
        }

    }
}