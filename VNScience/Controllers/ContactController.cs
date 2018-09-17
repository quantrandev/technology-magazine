using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Controllers
{
    public class ContactController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        SystemInfoDAO systemInfoDAO;

        public ContactController()
        {
            systemInfoDAO = new SystemInfoDAO(db);
        }

        [HttpGet]
        public ActionResult Create()
        {
            //get contact info
            ViewBag.ContactInfo = systemInfoDAO.GetContactInfo();

            return View();
        }

        [HttpPost]
        public ActionResult Create(Contact contact)
        {
            contact.CreatedAt = DateTime.Now;
            contact.IsSeen = false;

            db.Contacts.Add(contact);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}