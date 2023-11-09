using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Areas.Admin.Controllers
{
    public class DashBoardController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        // GET: Admin/DashBoard
        public ActionResult DashBoard()
        {
            var model = new DashBoard();
            model.TotalUsers = db.NGUOIDUNGs.Count();
            model.TotalAdmin = db.ADMINs.Count();
            model.TotalNewOrders = db.DONHANGs.Where(x => x.IDDONHANG == 1).Count();
            model.TotalOrders = db.DONHANGs.Where(x => x.IDDONHANG == 6).Count();
            model.TotalProducts = db.SANPHAMs.Count();
            model.TotalRevenue = db.DONHANGs.Where(x => x.IDDONHANG == 6)
                                     .Sum(x => (decimal?)x.THANHTIEN) ?? 0;

            return View(model);
        }
    }
}