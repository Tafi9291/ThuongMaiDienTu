using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        public ActionResult SanPham()
        {
            return View();
        }

        // GET: SanPham/Create
        public ActionResult TaoSanPham() 
        {
            return View();
        }
    }
}