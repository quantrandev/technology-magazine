﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Common;
using VNScience.Models;
using VNScience.ViewModels;

namespace VNScience.Areas.Admin.Controllers
{
    public class SystemInfoController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        SystemInfoDAO systemInfoDAO;

        public SystemInfoController()
        {
            systemInfoDAO = new SystemInfoDAO(db);
        }

        // GET: Admin/SystemInfo
        public ActionResult Index()
        {
            var model = new SystemInfoViewModel()
            {
                Logo = systemInfoDAO.GetLogo(),
                Brand = systemInfoDAO.GetBrand(),
                RecruitmentInfo = systemInfoDAO.GetRecruitmentInfo(),
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditBrand(SystemInfoViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            bool isSuccess = systemInfoDAO.UpdateBrand(model.Brand);

            if (isSuccess)
                Notification.Success("Đã cập nhật thành công tên công ty", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditRecruitmentInfo(SystemInfoViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            bool isSuccess = systemInfoDAO.UpdateRecruimentInfo(model.RecruitmentInfo);

            if (isSuccess)
                Notification.Success("Đã cập nhật thành công thông tin liên hệ tuyển dụng", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditLogo(SystemInfoViewModel model)
        {
            bool isSuccess = true;

            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            //upload file
            var uploadResult = UploadFile(Constants.AdminImagesUrl);
            if (uploadResult.Length != 0)
            {
                isSuccess = systemInfoDAO.UpdateLogo(uploadResult[0]);
                if (isSuccess)
                    Notification.Success("Đã cập nhật thành công logo", Session);
                else
                    Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);
            }
            else
            {
                Notification.Warning("Vui lòng chọn ảnh", Session);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditContactInfo(SystemInfoViewModel model)
        {
            bool isSuccess = true;

            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            isSuccess = systemInfoDAO.UpdateContactInfo(model.ContactInfo);
            if (isSuccess)
                Notification.Success("Đã cập nhật thành công thông tin liên hệ", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);
            return RedirectToAction("Index");
        }
    }
}