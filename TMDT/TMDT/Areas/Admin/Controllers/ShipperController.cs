using PagedList;
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

        public ActionResult QLDonHangShip(int? size, int? page, string currentFilter, string searchString)
        {
            SANPHAM sp = new SANPHAM();
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Shipper");
            }
            var orders = db.DONHANGs
                        .Where(s => s.IDTRANGTHAIDH != 1)
                        .OrderByDescending(p => p.IDDONHANG);

            // Thực hiện tìm kiếm nếu có chuỗi tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = (IOrderedQueryable<DONHANG>)orders.Where(p => p.NGUOINHAN.Contains(searchString));
            }

            // Lưu trạng thái tìm kiếm hiện tại
            currentFilter = searchString;

            var pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var pagedVouchers = orders
                .ToPagedList(pageNumber, pageSize);

            return View(pagedVouchers);
        }

        public ActionResult SelectCateTT()
        {
            TRANGTHAIDH se_cate = new TRANGTHAIDH();

            se_cate.ListCateTT = db.TRANGTHAIDHs.Where(t => t.IDTRANGTHAIDH == 2 || t.IDTRANGTHAIDH == 4 
            || t.IDTRANGTHAIDH == 5 || t.IDTRANGTHAIDH == 6 || t.IDTRANGTHAIDH == 7 || t.IDTRANGTHAIDH == 8).ToList<TRANGTHAIDH>();
            return PartialView(se_cate);
        }

        public ActionResult CapNhat(int id)
        {
            var editing = db.DONHANGs.Find(id);
            return View(editing);
        }

        [HttpPost]
        public ActionResult CapNhat(DONHANG model)
        {
            try
            {
                var sua = db.DONHANGs.Find(model.IDDONHANG);
                sua.IDTRANGTHAIDH = model.IDTRANGTHAIDH;
                sua.IDPTTHANHTOAN = model.IDPTTHANHTOAN;
                sua.NGUOINHAN = model.NGUOINHAN;
                sua.DIACHI = model.DIACHI;
                sua.SDT = model.SDT;
                db.SaveChanges();
                return RedirectToAction("QLDonHang");
            }
            catch
            {
                return View();
            }
        }
    }
}