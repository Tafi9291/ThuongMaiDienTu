using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class NgBanController : Controller
    {
        // GET: NgBan
        public ActionResult KenhNgBan()
        {
            return View();
        }

        // GET: NgBan
        public ActionResult DangNhapNgBan()
        {
            return View();
        }

        // GET: TaiKhoan
        public ActionResult TaiKhoan()
        {
            return View();
        }

        // GET: Welcome
        public ActionResult WelComeBanHang()
        {
            return View();
        }

        // GET: DangKyNgBan
        public ActionResult DangKyBanHang()
        {
            return View();
        }
    }
}