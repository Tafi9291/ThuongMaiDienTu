using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class MauSacController : Controller
    {
        // GET: Admin/MauSac
        public ActionResult MauSac()
        {
            return View();
        }

        // GET: Admin/MauSac/TaoMauSac
        public ActionResult TaoMauSac()
        {
            return View();
        }
    }
}