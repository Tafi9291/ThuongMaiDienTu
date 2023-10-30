using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult ShowToCart()
        {
            return View();
        }

        // GET: ChuaCoHang
        public ActionResult ChuaCoHang()
        {
            return View();
        }

        public ActionResult Complete()
        {
            return View();
        }

        public ActionResult CheckOut()
        {
            return View();
        }
    }
}