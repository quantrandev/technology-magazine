using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.ViewModels;
using VNScience.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using VNScience.Common;

namespace VNScience.Areas.Admin.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ProfileController()
        {
        }

        public ProfileController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Admin/Profile
        public ActionResult Index()
        {
            var loggedUser = db.Users.Find(User.Identity.GetUserId());

            ProfileViewModel model = new ProfileViewModel()
            {
                UserInfo = new UserInfoViewModel()
                {
                    Avatar = loggedUser.Avatar,
                    FullName = loggedUser.FullName
                }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateInfo(UserInfoViewModel UserInfo)
        {
            if (string.IsNullOrEmpty(UserInfo.FullName))
                return View();

            var editedUsers = db.Users.Find(User.Identity.GetUserId());
            editedUsers.FullName = UserInfo.FullName;
            db.SaveChanges();

            Notification.Success("Đã cập nhật thành công thông tin cá nhân", Session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateAvatar()
        {
            //upload file
            var path = "";
            var file = Request.Files[0];
            if (file.ContentLength > 0)
            {
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath(Common.Constants.AdminImagesUrl), fileName);
                    file.SaveAs(path);

                    //add to database
                    var editedUser = db.Users.Find(User.Identity.GetUserId());
                    editedUser.Avatar = Path.Combine(Common.Constants.AdminImagesUrl, fileName);
                    db.SaveChanges();
                }
                catch (Exception e) { }
            }

            Notification.Success("Đã cập nhật thành công avatar", Session);
            return RedirectToAction("Index");
        }

        //POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel Password)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", Password);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), Password.OldPassword, Password.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                //notifications
                Notification.Success("Đã đổi thành công mật khẩu", Session);


                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);

            Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);

            return RedirectToAction("Index", Password);
        }


        #region helpers
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