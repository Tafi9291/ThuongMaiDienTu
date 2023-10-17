using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class ThuongHieuController : Controller
    {
        // GET: Admin/ThuongHieu
        public ActionResult ThuongHieu()
        {
            return View();
        }

        // GET: Admin/ThuongHieu/TaoMoi
        public ActionResult TaoMoi() 
        {
            return View();
        }
    }
}