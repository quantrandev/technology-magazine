using AutoMapper;
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
using VNScience.ViewModels;
using System.Data.Entity;

namespace VNScience.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleName.SystemMod)]
    public class MenuController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        MenuTypeDAO menuTypeDAO;
        MenuDAO menuDAO;

        public MenuController()
        {
            menuTypeDAO = new MenuTypeDAO(db);
            menuDAO = new MenuDAO(db);
        }

        // GET: Admin/Menu
        public ActionResult Index(string searchString = null)
        {
            List<MenuViewModel> model = new List<MenuViewModel>();
            List<Menu> menus;

            if (string.IsNullOrEmpty(searchString))
            {
                menus = menuDAO.GetAllWithTypeAndUser();
            }
            else
            {
                menus = menuDAO.Search(searchString);
            }

            foreach (var item in menus)
            {
                model.Add(Mapper.Map<MenuViewModel>(item));
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            //menutypes 
            var menuTypes = menuTypeDAO.GetAll();
            MySelectList menuTypeSelectList = new MySelectList();
            menuTypeSelectList.FormElementName = "MenuTypeId";
            foreach (var item in menuTypes)
            {
                menuTypeSelectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            //other menu with displayOrder
            var otherMenus = db.Menus.Include(e => e.MenuType).GroupBy(e => e.MenuType).ToList();

            ViewBag.MenuTypeSelectList = menuTypeSelectList;
            ViewBag.OtherMenus = otherMenus;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Menu menu)
        {
            if (!ModelState.IsValid)
                return View();

            menu.CreatedAt = DateTime.Now;
            menu.CreatedBy = User.Identity.GetUserId();

            bool isSuccess = menuDAO.Insert(menu);

            if (isSuccess)
                Notification.Success("Đã thêm thành công menu mới", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var editedMenu = menuDAO.Get(id);

            //menutypes 
            var menuTypes = menuTypeDAO.GetAll();
            MySelectList menuTypeSelectList = new MySelectList();
            menuTypeSelectList.FormElementName = "MenuTypeId";
            foreach (var item in menuTypes)
            {
                if (item.Id == editedMenu.MenuTypeId)
                {
                    menuTypeSelectList.SelectedItems.Add(item.Id.ToString());
                }

                menuTypeSelectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            //other menu with displayOrder
            var otherMenus = db.Menus
                .Include(e => e.MenuType)
                .Where(e => e.Id != id)
                .GroupBy(e => e.MenuType)
                .ToList();

            ViewBag.MenuTypeSelectList = menuTypeSelectList;
            ViewBag.OtherMenus = otherMenus;
            return View(editedMenu);
        }

        [HttpPost]
        public ActionResult Edit(Menu menu)
        {
            if (!ModelState.IsValid)
                return View();

            menu.UpdatedAt = DateTime.Now;
            menu.UpdatedBy = User.Identity.GetUserId();

            bool isSuccess = menuDAO.Update(menu);

            if (isSuccess)
                Notification.Success("Đã cập nhật thành công menu", Session);
            else
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool isSuccess = menuDAO.Delete(id);

            return Json(new { status = isSuccess ? 200 : 500 }, JsonRequestBehavior.AllowGet);
        }
    }
}