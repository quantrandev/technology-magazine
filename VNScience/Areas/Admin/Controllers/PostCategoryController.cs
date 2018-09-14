using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Common;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.Controllers
{   
    public class PostCategoryController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        PostCategoryDAO postCategoryDAO;

        public PostCategoryController()
        {
            postCategoryDAO = new PostCategoryDAO(db);
        }

        // GET: Admin/PostCategory
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Index()
        {
            var postCategories = db.PostCategories
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .ToList();
            return View(postCategories);
        }

        [HttpGet]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Create()
        {
            //get all current display order
            var allPostCategories = db.PostCategories
                .OrderBy(e => e.DisplayOrder)
             .ToList();
            ViewBag.Categories = allPostCategories;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Create(PostCategory model)
        {
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = User.Identity.GetUserId();
            model.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? StringHelper.ToUnsignString(model.Name).ToLower() : model.MetaTitle.ToLower();

            db.PostCategories.Add(model);
            db.SaveChanges();

            Notification.Success("Đã thêm thành công danh mục bài viết", Session);

            //get all current display order
            var allPostCategories = db.PostCategories
                .OrderBy(e => e.DisplayOrder)
             .ToList();
            ViewBag.Categories = allPostCategories;

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Edit(int id)
        {
            var editedCategory = db.PostCategories.Find(id);
            //get all current display order
            var allPostCategories = db.PostCategories
                .OrderBy(e => e.DisplayOrder)
             .ToList();
            ViewBag.Categories = allPostCategories;

            return View(editedCategory);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Edit(PostCategory model)
        {
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = User.Identity.GetUserId();

            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            Notification.Success("Đã cập nhật thành công danh mục bài viết", Session);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = RoleName.PostMod)]
        public JsonResult Delete(int id)
        {
            bool isSuccess = postCategoryDAO.MarkAsDelete(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Details(int id)
        {
            var model = db.PostCategories
                .Include(e => e.UpdatingUser)
                .Include(e => e.CreatingUser)
                .FirstOrDefault(e => e.Id == id);
            return View(model);
        }
        

        #region FOR ADMIN
        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public JsonResult Destroy(int id)
        {
            bool isSuccess = postCategoryDAO.Destroy(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public JsonResult RefuseRequestToDelete(int id)
        {
            bool isSuccess = postCategoryDAO.UnDeleteRequest(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult RequestsToDelete()
        {
            var requestsToDelete = postCategoryDAO.GetManyRequestsToDelete();
            return View(requestsToDelete);
        }

        #endregion
    }
}