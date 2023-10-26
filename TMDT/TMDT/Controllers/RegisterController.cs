using System;
using System.Collections.Generic;
using System.IO;
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
            Session["Hinh"] = objUserGet.HINH;
            return RedirectToAction("TrangChu", "NgMua");

        }
        public ActionResult DangXuat()
        {
            Session["Email"] = null;
            return RedirectToAction("TrangChu", "NgMua");
        }
        public ActionResult DetailUser()
        {
            // Lấy thông tin khách hàng từ database
            var email = Session["Email"] as string;
            var customer = db.NGUOIDUNGs.FirstOrDefault(c => c.EMAIL == email);
            // Truyền thông tin khách hàng sang
            return View(customer);
        }
        public ActionResult EditUser(int id)
        {
            //Lấy thông tin khách hàng từ database
            var email = Session["Email"] as string;
            return View(db.NGUOIDUNGs.Where(s => s.IDND == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult EditUser(NGUOIDUNG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.NGUOIDUNGs.Find(model.IDND);
                //var edituser = db.KHACHHANGs.Where(x => x.KHACHHANGID == kh.KHACHHANGID).FirstOrDefault();
                if (model.UploadImage1 != null)
                {
                    string filename1 = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                    string extension1 = Path.GetExtension(model.UploadImage1.FileName);
                    filename1 = filename1 + extension1;
                    model.HINH = "~/Content/Hinh/" + filename1;
                    model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
                    // gan cac du lieu vao cai lay len

                }

                objUser.IDND = model.IDND;
                objUser.EMAIL = model.EMAIL;
                objUser.TENND = model.TENND;
                objUser.SDT = model.SDT;
                objUser.DIACHI = model.DIACHI;
                objUser.NHAPLAIMK = model.NHAPLAIMK;
                objUser.HINH = model.HINH;
                Session["Email"] = model.EMAIL;
                Session["Ten"] = model.TENND;
                Session["Hinh"] = model.HINH;
                db.SaveChanges();
                return RedirectToAction("DetailUser", "Register");
            }

            return View(model);
        }
    }
}