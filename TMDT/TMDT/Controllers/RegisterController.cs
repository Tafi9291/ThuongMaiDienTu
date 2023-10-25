using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
namespace TMDT.Controllers
{
    public class RegisterController : Controller
    {
        TMDTEntities db=new TMDTEntities();
        // GET: Register
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(NGUOIDUNG user)
        {
            if (ModelState.IsValid)
            {
                if (db.NGUOIDUNGs.Any(u => u.EMAIL == user.EMAIL))
                {
                    ViewBag.error = "Email đã tồn tại!";
                    return View();
                }

                if (user.MATKHAU != user.NHAPLAIMK)
                {
                    ViewBag.error1 = "Mật khẩu không trùng khớp!";
                    return View();
                }
                user.IDPQND = 1;
                db.NGUOIDUNGs.Add(user);
                db.SaveChanges();

                return RedirectToAction("DangNhap", "Register");
            }

            return View(user);
        }
        // GET: DangNhap
        public ActionResult DangNhap()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(NGUOIDUNG user)
        {

            var objUserGet = db.NGUOIDUNGs.Where(n => n.EMAIL.Equals(user.EMAIL) && n.MATKHAU.Equals(user.MATKHAU)).FirstOrDefault();


            if (objUserGet == null)
            {
                ViewBag.error = "Email hay Mật khẩu không đúng vui lòng nhập lại!";
                return View();
            }
            else
            Session["Email"] = user.EMAIL;
            Session["Ten"] = objUserGet.TENND;
            //Session["Hinh"] = objUserGet.HINH;
            return RedirectToAction("TrangChu", "NgMua");

        }
        public ActionResult DangXuat()
        {
            Session["Email"] = null;
            return RedirectToAction("TrangChu", "NgMua");
        }
    }
}