using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class VoucherShopController : Controller
    {
        // GET: VoucherShop
        public ActionResult QLVoucherShop()
        {
            return View();
        }

        // GET: Tao moi voucher
        public ActionResult TaoVoucher() 
        {
            return View();
        }


        // GET: Cap nhat voucher
        public ActionResult CapNhatVoucher()
        {
            return View();
        }
    }
}