﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class QLNDController : Controller
    {
        // GET: Admin/QLND
        public ActionResult QLNgMua()
        {
            return View();
        }

        // GET
        public ActionResult QLNgBan()
        {
            return View();
        }
    }
}