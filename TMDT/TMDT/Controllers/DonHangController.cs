﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class DonHangController : Controller
    {
        // GET: DonHang
        public ActionResult QLDonHang()
        {
            return View();
        }


        // GET: Edit DonHang
        public ActionResult CapNhat()
        {
            return View();
        }

        // GET: Detail DonHang
        public ActionResult CTDonHang()
        {
            return View();
        }
    }
}