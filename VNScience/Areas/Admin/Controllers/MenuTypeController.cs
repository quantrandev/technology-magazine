using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Models;

namespace VNScience.Areas.Admin.Controllers
{
    public class MenuTypeController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        MenuTypeDAO menuTypeDAO;

        public MenuTypeController()
        {
            menuTypeDAO = new MenuTypeDAO(db);
        }

        // GET: Admin/MenuType
        public ActionResult Index()
        {
            var menuTypes = menuTypeDAO.GetAll();

            return View(menuTypes);
        }
    }
}