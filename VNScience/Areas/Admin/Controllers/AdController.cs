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
    public class AdController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        AdDAO adDAO;
        public AdController()
        {
            adDAO = new AdDAO(db);
        }

        // GET: Admin/Slide
        public ActionResult Index()
        {
            var ads = adDAO.GetAll();

            return View(ads);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.OtherAds = adDAO.GetAll();
            ViewBag.Positions = new MySelectList()
            {
                FormElementName = "Position",
                Items = new List<MySelectListItem>
                {
                    new MySelectListItem(){Id=((int)AdPosition.TopCenter).ToString(), Name = AdPosition.TopCenter.ToString()},
                    new MySelectListItem(){Id=((int)AdPosition.TopLeft).ToString(), Name = AdPosition.TopLeft.ToString()},
                    new MySelectListItem(){Id=((int)AdPosition.BottomLeft).ToString(), Name = AdPosition.BottomLeft.ToString()}
                }
            };

            return View();
        }

        [HttpPost]
        public ActionResult Create(Ad model)
        {
            if (!ModelState.IsValid)
                return View();

            //upload file
            var uploadResult = UploadFile(Common.Constants.AdminImagesUrl);
            model.Content = uploadResult.Length == 0 ? "" : uploadResult[0];

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = User.Identity.GetUserId();

            bool isSuccess = adDAO.Insert(model);

            if (isSuccess)
                Notification.Success("Đã thêm thành công quảng cáo mới", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var editedAd = adDAO.Get(id);

            ViewBag.OtherAds = adDAO.GetAllExcept(id);

            var positionSelectList = new MySelectList()
            {
                FormElementName = "Position",
                Items = new List<MySelectListItem>
                {
                    new MySelectListItem(){Id=((int)AdPosition.TopCenter).ToString(), Name = AdPosition.TopCenter.ToString()},
                    new MySelectListItem(){Id=((int)AdPosition.TopLeft).ToString(), Name = AdPosition.TopLeft.ToString()},
                    new MySelectListItem(){Id=((int)AdPosition.BottomLeft).ToString(), Name = AdPosition.BottomLeft.ToString()}
                }
            };

            if (editedAd.Position == (int)AdPosition.TopCenter)
                positionSelectList.SelectedItems.Add(((int)AdPosition.TopCenter).ToString());
            if (editedAd.Position == (int)AdPosition.TopLeft)
                positionSelectList.SelectedItems.Add(((int)AdPosition.TopLeft).ToString());
            if (editedAd.Position == (int)AdPosition.BottomLeft)
                positionSelectList.SelectedItems.Add(((int)AdPosition.BottomLeft).ToString());

            ViewBag.Positions = positionSelectList;
            return View(editedAd);
        }

        [HttpPost]
        public ActionResult Edit(Ad model)
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

            bool isSuccess = adDAO.Update(model);

            if (isSuccess)
                Notification.Success("Đã cập nhật thành công quảng cáo", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool isSuccess = adDAO.Delete(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }
    }
}