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
    public class ProductCategoryController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ProductCategoryDAO productCategoryDAO;
        public ProductCategoryController()
        {
            productCategoryDAO = new ProductCategoryDAO(db);
        }

        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var categories = productCategoryDAO.GetAll();
            return View(categories);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var categories = productCategoryDAO.GetAll();

            //parent selectlist
            var parentSelectList = new MySelectList()
            {
                FormElementName = "ParentId"
            };
            parentSelectList.Items.Add(new MySelectListItem()
            {
                Id = "",
                Name = "Chọn danh mục cha"
            });
            foreach (var item in categories)
            {
                parentSelectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            ViewBag.Parents = parentSelectList;
            ViewBag.OtherCategories = productCategoryDAO.GetAll().ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ProductCategory model)
        {
            if (!ModelState.IsValid)
                return View();

            //upload file
            var uploadResult = UploadFile(Common.Constants.AdminImagesUrl);
            model.CoverImage = uploadResult.Length == 0 ? "" : uploadResult[0];

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = User.Identity.GetUserId();

            bool isSuccess = productCategoryDAO.Insert(model);

            if (isSuccess)
                Notification.Success("Đã thêm thành công danh mục giải pháp mới", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var editedCategory = productCategoryDAO.Get(id);

            var categories = productCategoryDAO.GetAll();

            //parent selectlist
            var parentSelectList = new MySelectList()
            {
                FormElementName = "ParentId"
            };
            parentSelectList.Items.Add(new MySelectListItem()
            {
                Id = "",
                Name = "Chọn danh mục cha"
            });
            foreach (var item in categories)
            {
                if (item.Id == editedCategory.ParentId)
                    parentSelectList.SelectedItems.Add(item.Id.ToString());

                parentSelectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            ViewBag.Parents = parentSelectList;
            ViewBag.OtherCategories = productCategoryDAO.GetAllExcept(id).ToList();
            return View(editedCategory);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ProductCategory model)
        {
            if (!ModelState.IsValid)
                return View();

            //process file
            if (Request.Files[0].ContentLength > 0)
            {
                var uploadResult = UploadFile(Common.Constants.AdminImagesUrl);
                model.CoverImage = uploadResult.Length == 0 ? "" : uploadResult[0];
            }

            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = User.Identity.GetUserId();

            bool isSuccess = productCategoryDAO.Update(model);

            if (isSuccess)
                Notification.Success("Đã cập nhật thành công danh mục giải pháp", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool isSuccess = productCategoryDAO.Delete(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var product = productCategoryDAO.Get(id);

            return View(product);
        }
    }
}