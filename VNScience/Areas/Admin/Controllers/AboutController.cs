using Microsoft.AspNet.Identity;
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
    public class AboutController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        AboutDAO aboutDAO;

        public AboutController()
        {
            aboutDAO = new AboutDAO(db);
        }
        // GET: Admin/About
        public ActionResult Index()
        {
            var model = aboutDAO.GetAllWithUser();

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(About about)
        {
            if (!ModelState.IsValid)
                return View();

            about.CreatedAt = DateTime.Now;
            about.CreatedBy = User.Identity.GetUserId();

            bool isSuccess = aboutDAO.Insert(about);

            if (isSuccess)
                Notification.Success("Đã thêm thành công tin giới thiệu", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại", Session);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var editedAbout = aboutDAO.Get(id);

            return View(editedAbout);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(About about)
        {
            if (!ModelState.IsValid)
                return View();

            about.UpdatedAt = DateTime.Now;
            about.UpdatedBy = User.Identity.GetUserId();

            bool isSuccess = aboutDAO.Update(about);

            if (isSuccess)
                Notification.Success("Đã cập nhật thành công tin giới thiệu", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool isSuccess = aboutDAO.Destroy(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Content(int id)
        {
            var content = aboutDAO.Get(id).Content;

            return Json(new { status = 200, data = content }, JsonRequestBehavior.AllowGet);
        }
    }
}