using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
namespace TMDT.Areas.Admin.Controllers
{
    public class DangNhapAdminController : Controller
    {
        TMDTEntities db=new TMDTEntities();
        // GET: Admin/DangNhap
        public ActionResult DangNhapAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapAdmin(ADMIN admin)
        {
            var objUserGet = db.ADMINs.Where(n => n.EMAIL.Equals(admin.EMAIL) && n.MATKHAU.Equals(admin.MATKHAU)).FirstOrDefault();
           
            if (objUserGet == null)
            {
                ViewBag.error = "Email hay Mật khẩu không đúng vui lòng nhập lại!";
                return View();
            }
            else
            Session["EmailAD"] = objUserGet.EMAIL;
            Session["TenAD"] = objUserGet.TENAD;
            Session["HinhAD"] = objUserGet.HINHANH;
            return RedirectToAction("DashBoard", "DashBoard");


        }
        public ActionResult DangXuat()
        {
            Session["EmailAD"] = null;
            return RedirectToAction("DangNhapAdmin", "DangNhapAdmin");
        }
    }
}