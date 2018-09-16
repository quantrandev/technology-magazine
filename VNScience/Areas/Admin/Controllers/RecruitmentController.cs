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
    public class RecruitmentController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        RecruitmentDAO recruitmentDAO;
        public RecruitmentController()
        {
            recruitmentDAO = new RecruitmentDAO(db);
        }

        // GET: Admin/Recruitment
        public ActionResult Index(string searchString = null)
        {
            List<Recruitment> recruitments;

            if (string.IsNullOrEmpty(searchString))
            {
                recruitments = recruitmentDAO.GetAllWithUser();
            }
            else
            {
                recruitments = recruitmentDAO.Search(searchString);
            }
            
            return View(recruitments);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Recruitment model)
        {
            if (!ModelState.IsValid)
                return View();

            model.MetaTitle = StringHelper.ToUnsignString(model.Title);
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = User.Identity.GetUserId();

            bool isSuccess = recruitmentDAO.Insert(model);

            if (isSuccess)
                Notification.Success("Đã thêm thành công tin tuyển dụng", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var editedRecruitment = recruitmentDAO.Get(id);
            return View(editedRecruitment);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Recruitment model)
        {
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = User.Identity.GetUserId();

            bool isSuccess = recruitmentDAO.Update(model);

            if (isSuccess)
                Notification.Success("Đã cập nhật thành công tin tuyển dụng", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool isSuccess = recruitmentDAO.Delete(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(long id)
        {
            var recruitment = recruitmentDAO.GetWithUser(id);
            return View(recruitment);
        }
    }
}