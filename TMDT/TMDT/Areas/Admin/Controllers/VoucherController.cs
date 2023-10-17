using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class VoucherController : Controller
    {
        // GET: Admin/Voucher
        public ActionResult Voucher()
        {
            return View();
        }

        // GET: Admin/Voucher/TaoVoucher
        public ActionResult TaoVoucher()
        {
            return View();
        }

        // GET: Admin/Voucher/CapNhatVC
        public ActionResult CapNhatVC()
        {
            return View();
        }
    }
}