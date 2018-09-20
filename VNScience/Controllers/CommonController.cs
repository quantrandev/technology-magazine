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
        AdDAO adDAO;

        public CommonController()
        {
            postCategoryDAO = new PostCategoryDAO(db);
            menuDAO = new MenuDAO(db);
            postDAO = new PostDAO(db);
            systemInfoDAO = new SystemInfoDAO(db);
            adDAO = new AdDAO(db);
        }

        public PartialViewResult Header()
        {
            ViewBag.Logo = systemInfoDAO.GetLogo();
            ViewBag.TopMenus = menuDAO.GetTopMenus();
            ViewBag.Categories = postCategoryDAO.GetAll();
            ViewBag.SocialLinks = systemInfoDAO.GetSocialLink();
            return PartialView("Header");
        }

        public PartialViewResult Footer()
        {
            ViewBag.Logo = systemInfoDAO.GetLogo();
            ViewBag.BottomMenus = menuDAO.GetBottomMenus();
            ViewBag.SocialLinks = systemInfoDAO.GetSocialLink();
            ViewBag.ContactInfo = systemInfoDAO.GetContactInfo();
            ViewBag.MostViews = postDAO.MostViews(1, 2);
            ViewBag.Recents = postDAO.GetAllWithUserAndCategory().Skip(0).Take(2).ToList();
            return PartialView("Footer");
        }

        public PartialViewResult RightCol()
        {
            ViewBag.Ads = adDAO.GetAll();

            return PartialView("RightCol");
        }

        public PartialViewResult TopCenter()
        {
            ViewBag.Ads = adDAO.GetAll();

            return PartialView("TopCenter");
        }
    }
}