using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Common;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleName.SystemMod)]
    public class ContactController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ContactDAO contactDAO;

        public ContactController()
        {
            contactDAO = new ContactDAO(db);
        }

        // GET: Admin/Contact
        public ActionResult Index(string searchString = null)
        {
            List<Contact> contacts;

            if (string.IsNullOrEmpty(searchString))
            {
                contacts = contactDAO.GetAll();
            }
            else
            {
                contacts = contactDAO.Search(searchString);
            }

            return View(contacts);
        }
        [HttpPost]
        public JsonResult Delete(long id)
        {
            bool isSuccess = contactDAO.Destroy(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Seen(long id)
        {
            bool isSuccess = contactDAO.MarkAsSeen(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }
    }
}