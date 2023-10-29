using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Areas.Admin.Controllers
{
    public class ShipperController : Controller
    {
        TMDTEntities db=new TMDTEntities();
        // GET: Admin/Shipper
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(SHIPPER shipper)
        {
            var objUserGet = db.SHIPPERs.Where(n => n.EMAIL.Equals(shipper.EMAIL) && n.MATKHAU.Equals(shipper.MATKHAU)).FirstOrDefault();

            if (objUserGet == null)
            {
                ViewBag.error = "Email hay Mật khẩu không đúng vui lòng nhập lại!";
                return View();
            }
            else
            Session["Email"] = objUserGet.EMAIL;
            Session["Ten"] = objUserGet.TEN;
            
            return RedirectToAction("TaiKhoan", "Shipper");


        }
        public ActionResult TaiKhoan()
        {
            // Lấy thông tin khách hàng từ database
            var email = Session["Email"] as string;
            var shipper = db.SHIPPERs.FirstOrDefault(c => c.EMAIL == email);
            // Truyền thông tin khách hàng sang
            return View(shipper);
        }
        [HttpPost]
        [ActionName("CapNhatThongTin")]
        public ActionResult CapNhatThongTin(SHIPPER model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.SHIPPERs.Find(model.IDSHIPPER);

              
                objUser.TEN = model.TEN;
                objUser.DIACHI = model.DIACHI;
                Session["Ten"] = model.TEN;
               
                db.SaveChanges();
                return RedirectToAction("TaiKhoan", "Shipper");
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("SDT")]
        public ActionResult SDT(SHIPPER model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.SHIPPERs.Find(model.IDSHIPPER);
                objUser.SDT = model.SDT;
                db.SaveChanges();
                return RedirectToAction("TaiKhoan", "Shipper");
            }

            return View(model);
        }



        [HttpPost]
        [ActionName("MatKhau")]
        public ActionResult MatKhau(SHIPPER model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.SHIPPERs.Find(model.IDSHIPPER);
                objUser.MATKHAU = model.MATKHAU;
                db.SaveChanges();
                return RedirectToAction("TaiKhoan", "Shipper");
            }

            return View(model);
        }

        public ActionResult DangXuat()
        {
            Session["Email"] = null;
            return RedirectToAction("DangNhap", "Shipper");
        }
    }
}