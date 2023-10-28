using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
namespace TMDT.Areas.Admin.Controllers
{
    public class TaiKhoanController : Controller
    {
        TMDTEntities db=new TMDTEntities();
        // GET: Admin/TaiKhoan
        public ActionResult TaiKhoan()
        {
            // Lấy thông tin khách hàng từ database
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhapAdmin", "DangNhapAdmin");
            }
            var adnin = db.ADMINs.FirstOrDefault(c => c.EMAIL == email);
            // Truyền thông tin khách hàng sang
            return View(adnin);
        }
        [HttpPost]
        [ActionName("CapNhatThongTin")]
        public ActionResult CapNhatThongTin(ADMIN model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.ADMINs.Find(model.IDADMIN);
               
                if (model.UploadImage1 != null)
                {
                    string filename1 = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                    string extension1 = Path.GetExtension(model.UploadImage1.FileName);
                    filename1 = filename1 + extension1;
                    model.HINHANH = "~/Content/Hinh/" + filename1;
                    model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
                    // gan cac du lieu vao cai lay len

                }
                objUser.TENAD = model.TENAD;
                objUser.HINHANH = model.HINHANH;
                Session["Ten"] = model.TENAD;
                Session["Hinh"] = model.HINHANH;
                db.SaveChanges();
                return RedirectToAction("TaiKhoan", "TaiKhoan");
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("SDT")]
        public ActionResult SDT(ADMIN model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.ADMINs.Find(model.IDADMIN);
                objUser.SDT = model.SDT;
                db.SaveChanges();
                return RedirectToAction("TaiKhoan", "TaiKhoan");
            }

            return View(model);
        }



        [HttpPost]
        [ActionName("MatKhau")]
        public ActionResult MatKhau(ADMIN model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.ADMINs.Find(model.IDADMIN);
                objUser.MATKHAU = model.MATKHAU;
                db.SaveChanges();
                return RedirectToAction("TaiKhoan", "TaiKhoan");
            }

            return View(model);
        }

    }
}