using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Common;
using VNScience.Models;

namespace VNScience.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected Notification Notification { get; set; }
        PostDAO postDAO; 

        public BaseController()
        {
            Notification = new Notification();
        }

        protected string[] UploadFile(string relativePath)
        {
            bool isSuccess = true;

            var path = new List<string>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (Request.Files[i].ContentLength > 0)
                {
                    try
                    {
                        var fileName = Path.GetFileName(Request.Files[i].FileName);
                        var savePath = Path.Combine(Server.MapPath(relativePath), fileName);
                        path.Add(Path.Combine(relativePath, fileName));
                        Request.Files[i].SaveAs(savePath);
                    }
                    catch (Exception e)
                    {
                        isSuccess = false;
                    }
                }
            }

            return path.ToArray();
        }
    }
}