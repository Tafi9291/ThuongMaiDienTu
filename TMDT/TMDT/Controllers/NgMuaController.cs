using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
namespace TMDT.Controllers
{
    public class NgMuaController : Controller
    {
        TMDTEntities db=new TMDTEntities();
        // GET: NgMua
        public ActionResult TrangChu()
        {

            return View();
        }

        // GET: NgMua/SanPham/Detail
        public ActionResult Detail()
        {
            return View();
        }
    }
}