using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Models;

namespace VNScience.Controllers
{
    public class CommonController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        PostDAO postDAO;
        SystemInfoDAO systemInfoDAO;
        MenuDAO menuDAO;
        PostCategoryDAO postCategoryDAO;

        public CommonController()
        {
            postCategoryDAO = new PostCategoryDAO(db);
            menuDAO = new MenuDAO(db);
            postDAO = new PostDAO(db);
            systemInfoDAO = new SystemInfoDAO(db);
        }

        public PartialViewResult Header()
        {
            ViewBag.Logo = systemInfoDAO.GetLogo();
            ViewBag.TopMenus = menuDAO.GetTopMenus();
            ViewBag.Categories = postCategoryDAO.GetAll();
            return PartialView("Header");
        }

        public PartialViewResult Footer()
        {
            ViewBag.Logo = systemInfoDAO.GetLogo();
            ViewBag.BottomMenus = menuDAO.GetBottomMenus();
            return PartialView("Footer");
        }

        public PartialViewResult RightCol()
        {
            return PartialView("RightCol");
        }
    }
}