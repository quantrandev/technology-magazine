using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        SlideDAO slideDAO;
        public SlideController()
        {
            slideDAO = new SlideDAO(db);
        }
        // GET: Admin/Slide
        public ActionResult Index()
        {
            var slides = slideDAO.GetAll();

            return View(slides);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.OtherSlides = slideDAO.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Slide model)
        {
            if (!ModelState.IsValid)
                return View();

            //upload file
            var uploadResult = UploadFile(Common.Constants.AdminImagesUrl);
            model.Content = uploadResult.Length == 0 ? "" : uploadResult[0];

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = User.Identity.GetUserId();

            bool isSuccess = slideDAO.Insert(model);

            if (isSuccess)
                Notification.Success("Đã thêm thành công slide mới", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var editedSlide = slideDAO.Get(id);

            ViewBag.OtherSlides = slideDAO.GetAllExcept(id);
            return View(editedSlide);
        }

        [HttpPost]
        public ActionResult Edit(Slide model)
        {
            if (!ModelState.IsValid)
                return View();

            //process file
            if (Request.Files[0].ContentLength > 0)
            {
                var uploadResult = UploadFile(Common.Constants.AdminImagesUrl);
                model.Content = uploadResult.Length == 0 ? "" : uploadResult[0];
            }

            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = User.Identity.GetUserId();

            bool isSuccess = slideDAO.Update(model);

            if (isSuccess)
                Notification.Success("Đã cập nhật thành công slide", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool isSuccess = slideDAO.Delete(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }
    }
}