using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class SizeController : Controller
    {
        // GET: Admin/Size
        public ActionResult Size()
        {
            return View();
        }

        // GET: Admin/Size/TaoSize
        public ActionResult TaoSize()
        {
            return View();
        }
    }
}