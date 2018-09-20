using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.ViewModels;

namespace VNScience.Areas.Admin.DataAccess
{
    public class SystemInfoDAO
    {
        ApplicationDbContext _db;
        public SystemInfoDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        //Get
        public string GetBrand()
        {
            return _db.SystemInfoes.Find("brand").Content;
        }

        public string GetLogo()
        {
            return _db.SystemInfoes.Find("logo").Content;
        }

        public string GetRecruitmentInfo()
        {
            return _db.SystemInfoes.Find("recruitmentInfo").Content;
        }

        public SystemInfoViewModel GetContactInfo()
        {
            var systemInfo = new SystemInfoViewModel()
            {
                Hotline = _db.SystemInfoes.Find("hotline").Content,
                Email = _db.SystemInfoes.Find("email").Content,
                Website = _db.SystemInfoes.Find("website").Content,
                CompanyFullName = _db.SystemInfoes.Find("companyfullname").Content,
                Brand = _db.SystemInfoes.Find("brand").Content,
                Headquater = _db.SystemInfoes.Find("headquater").Content
            };
            return systemInfo;
        }



        //Update
        public bool UpdateBrand(string newBrand)
        {
            bool isSuccess = true;
            var brand = _db.SystemInfoes.Find("brand");
            brand.Content = newBrand;

            try
            {
                _db.Entry(brand).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        public bool UpdateLogo(string newLogo)
        {
            bool isSuccess = true;
            var logo = _db.SystemInfoes.Find("logo");
            logo.Content = newLogo;

            try
            {
                _db.Entry(logo).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        public bool UpdateRecruimentInfo(string newRecruitmentInfo)
        {
            bool isSuccess = true;
            var recruitmentInfo = _db.SystemInfoes.Find("recruitmentInfo");
            recruitmentInfo.Content = newRecruitmentInfo;

            try
            {
                _db.Entry(recruitmentInfo).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        public bool UpdateContactInfo(SystemInfoViewModel model)
        {
            bool isSuccess = true;
            var hotline = _db.SystemInfoes.Find("hotline");
            hotline.Content = model.Hotline;
            var email = _db.SystemInfoes.Find("email");
            email.Content = model.Email;
            var website = _db.SystemInfoes.Find("website");
            website.Content = model.Website;
            var companyFullName = _db.SystemInfoes.Find("companyfullname");
            companyFullName.Content = model.CompanyFullName;
            var brand = _db.SystemInfoes.Find("brand");
            brand.Content = model.Brand;
            var headquater = _db.SystemInfoes.Find("headquater");
            headquater.Content = model.Headquater;
            try
            {
                _db.Entry(hotline).State = EntityState.Modified;
                _db.Entry(email).State = EntityState.Modified;
                _db.Entry(website).State = EntityState.Modified;
                _db.Entry(companyFullName).State = EntityState.Modified;
                _db.Entry(brand).State = EntityState.Modified;
                _db.Entry(headquater).State = EntityState.Modified;

                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public SystemInfoViewModel GetSocialLink()
        {
            var systemInfo = new SystemInfoViewModel()
            {
                Facebook = _db.SystemInfoes.Find("facebook").Content,
                Linkedin = _db.SystemInfoes.Find("linkedin").Content,
                Youtube = _db.SystemInfoes.Find("youtube").Content,
            };
            return systemInfo;
        }

        public bool UpdateSocialLink(SystemInfoViewModel model)
        {
            bool isSuccess = true;
            var facebook = _db.SystemInfoes.Find("facebook");
            facebook.Content = model.Facebook;
            var linkedin = _db.SystemInfoes.Find("linkedin");
            linkedin.Content = model.Linkedin;
            var youtube = _db.SystemInfoes.Find("youtube");
            youtube.Content = model.Youtube;

            try
            {
                _db.Entry(facebook).State = EntityState.Modified;
                _db.Entry(linkedin).State = EntityState.Modified;
                _db.Entry(youtube).State = EntityState.Modified;

                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}