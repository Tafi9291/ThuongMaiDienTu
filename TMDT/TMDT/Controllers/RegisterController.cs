using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult DangKy()
        {
            return View();
        }

        // GET: DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }
    }
}