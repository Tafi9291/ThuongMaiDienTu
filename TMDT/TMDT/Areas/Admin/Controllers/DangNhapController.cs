using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class DangNhapController : Controller
    {
        // GET: Admin/DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }

        // GET: Admin/TaiKhoan
        public ActionResult TaiKhoan()
        {
            return View();
        }
    }
}