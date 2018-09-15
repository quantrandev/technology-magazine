using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VNScience.Models;

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
            catch(Exception e)
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
    }
}