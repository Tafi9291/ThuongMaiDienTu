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
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(NGUOIDUNG user)
        {
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
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
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(NGUOIDUNG user)
        {

            var objUserGet = db.NGUOIDUNGs.Where(n => n.EMAIL.Equals(user.EMAIL) && n.MATKHAU.Equals(user.MATKHAU)).FirstOrDefault();
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;

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
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            // Truyền thông tin khách hàng sang
            return View(customer);
        }
        public ActionResult EditUser(int id)
        {
            //Lấy thông tin khách hàng từ database
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            var email = Session["Email"] as string;
            return View(db.NGUOIDUNGs.Where(s => s.IDND == id).FirstOrDefault());
        }
        [HttpPost]
        [ActionName("EditUser")]
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

                objUser.TENND = model.TENND;



                objUser.HINH = model.HINH;

                Session["Ten"] = model.TENND;
                Session["Hinh"] = model.HINH;
                db.SaveChanges();
                return RedirectToAction("DetailUser", "Register");
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("SDT")]
        public ActionResult SDT(NGUOIDUNG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.NGUOIDUNGs.Find(model.IDND);

                objUser.IDND = model.IDND;

                objUser.SDT = model.SDT;

                db.SaveChanges();
                return RedirectToAction("DetailUser", "Register");
            }

            return View(model);
        }



        [HttpPost]
        [ActionName("DiaChi")]
        public ActionResult DiaChi(NGUOIDUNG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.NGUOIDUNGs.Find(model.IDND);

                objUser.IDND = model.IDND;
                objUser.DIACHI = model.DIACHI;

                db.SaveChanges();
                return RedirectToAction("DetailUser", "Register");
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("Email")]
        public ActionResult Email(NGUOIDUNG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.NGUOIDUNGs.Find(model.IDND);

                objUser.IDND = model.IDND;
                objUser.EMAIL = model.EMAIL;

                db.SaveChanges();
                return RedirectToAction("DetailUser", "Register");
            }

            return View(model);
        }
    }
}