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
    public class ProductController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ProductDAO productDAO;
        ProductCategoryDAO productCategoryDAO;

        public ProductController()
        {
            productDAO = new ProductDAO(db);
            productCategoryDAO = new ProductCategoryDAO(db);
        }

        // GET: Admin/Product
        public ActionResult Index()
        {
            var products = productDAO.GetAll();

            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var productCategories = productCategoryDAO.GetAll();
            //categories selectlist
            var categorySelectList = new MySelectList()
            {
                FormElementName = "CategoryId"
            };
            foreach (var item in productCategories)
            {
                categorySelectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            ViewBag.Categories = categorySelectList;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product model)
        {
            if (!ModelState.IsValid)
                return View();

            //upload file
            var uploadResult = UploadFile(Common.Constants.AdminImagesUrl);
            model.CoverImage = uploadResult.Length == 0 ? "" : uploadResult[0];

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = User.Identity.GetUserId();

            bool isSuccess = productDAO.Insert(model);

            if (isSuccess)
                Notification.Success("Đã thêm thành công giải pháp mới", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }
    }
}