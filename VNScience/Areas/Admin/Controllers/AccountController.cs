using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Models;
using Microsoft.AspNet.Identity;
using VNScience.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.IO;
using VNScience.Common;
using Microsoft.AspNet.Identity.EntityFramework;
using VNScience.Models.Core;
using System.Threading.Tasks;
using System.Data.Entity;

namespace VNScience.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class AccountController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin/Account
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var users = db.Users.Where(e => e.Id != currentUserId).ToList();

            var model = new List<UserViewModel>();
            foreach (var user in users)
            {
                model.Add(new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Avatar = user.Avatar,
                    Email = user.Email,
                    Roles = UserManager.GetRoles(user.Id).ToArray()
                });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var rolesInDb = db.Roles.ToList();
            var selectList = new MySelectList()
            {
                FormElementName = "Roles"
            };
            foreach (var item in rolesInDb)
            {
                selectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Name,
                    Name = item.Name
                });
            }

            ViewBag.SelectList = selectList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //upload file
                var path = "";
                var fileName = "";
                var file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    try
                    {
                        fileName = Path.GetFileName(file.FileName);
                        path = Path.Combine(Server.MapPath(Common.Constants.AdminImagesUrl), fileName);
                        file.SaveAs(path);
                    }
                    catch (Exception e) { }
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName, Avatar = string.IsNullOrEmpty(path) ? "" : Path.Combine(Common.Constants.AdminImagesUrl, fileName) };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //add user to roles
                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    await UserManager.AddToRolesAsync(user.Id, model.Roles);

                    Notification.Success("Đã thêm thành công tài khoản cho người dùng: " + model.FullName, Session);
                    return RedirectToAction("Index", "Account", new { area = "Admin" });
                }
                AddErrors(result);

                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePassword(string id)
        {
            ResetPasswordViewModel changePasswordVM = new ResetPasswordViewModel()
            {
                UserId = id
            };
            return View(changePasswordVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            };

            var token = await UserManager.GeneratePasswordResetTokenAsync(model.UserId);
            var result = await UserManager.ResetPasswordAsync(model.UserId, token, model.NewPassword);
            if (result.Succeeded)
            {
                Notification.Success("Đã cập nhật thành công mật khẩu", Session);
                return RedirectToAction("Index", "Account", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);

            Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var editedUser = db.Users
                .FirstOrDefault(e => e.Id == id);

            var model = new UserViewModel()
            {
                Id = id,
                FullName = editedUser.FullName,
                Avatar = editedUser.Avatar,
                Roles = GetRolesOfUSer(id)
            };

            var rolesInDb = db.Roles.ToList();
            var selectList = new MySelectList()
            {
                FormElementName = "Roles",
                SelectedItems = GetRolesOfUSer(id).ToList()
            };
            foreach (var item in rolesInDb)
            {
                selectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Name,
                    Name = item.Name
                });
            }

            ViewBag.SelectList = selectList;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInfo(UserViewModel model)
        {
            var editedUser = db.Users.Find(model.Id);
            editedUser.FullName = model.FullName;
            db.SaveChanges();

            Notification.Success("Đã cập nhật thành công thông tin người dùng", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAvatar(UserViewModel model)
        {
            //upload file
            var path = "";
            var fileName = "";
            var file = Request.Files[0];
            if (file.ContentLength > 0)
            {
                try
                {
                    fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath(Common.Constants.AdminImagesUrl), fileName);
                    file.SaveAs(path);
                }
                catch (Exception e) { }
            }

            var editedUser = db.Users.Find(model.Id);
            editedUser.Avatar = string.IsNullOrEmpty(path) ? path : Path.Combine(Common.Constants.AdminImagesUrl, fileName);
            db.SaveChanges();

            Notification.Success("Đã cập nhật thành công avatar", Session);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GivePermission(UserViewModel model)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var userRoles = GetRolesOfUSer(model.Id);

            var addedRoles = model.Roles.Where(e => !userRoles.Contains(e)).ToArray();
            var deletedRoles = userRoles.Where(e => !model.Roles.Contains(e)).ToArray();

            userManager.AddToRoles(model.Id, addedRoles);
            userManager.RemoveFromRoles(model.Id, deletedRoles);

            Notification.Success("Đã cập nhật thành công", Session);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            db.Users.Remove(db.Users.Find(id));
            db.SaveChanges();

            return Json(new { status = 200 }, JsonRequestBehavior.AllowGet);
        }























        #region helpers
        private string[] GetRolesOfUSer(string userId)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var userRoles = userManager.GetRoles(userId);
            var selectedRoles = db.Roles.Where(e => userRoles.Contains(e.Name)).Select(e => e.Name).ToList().ToArray();

            return selectedRoles;
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
        #endregion
    }
}