using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class VoucherShopController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        // GET: VoucherShop
        public ActionResult QLVoucherShop(int? size, int? page, string currentFilter, string searchString)
        {
            VOUCHERSHOP vc = new VOUCHERSHOP();
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhapNgBan", "NgBan");
            }
            // Lấy thông tin của người dùng trong cơ sở dữ liệu
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null) // Kiểm tra xem người dùng có tồn tại hay không
            {
                // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
                var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
                {
                    vc.IDCUAHANG = cuahang.IDCUAHANG;

                    // Lấy danh sách sản phẩm theo IDCUAHANG
                    var vouchers = db.VOUCHERSHOPs
                        .Where(p => p.IDCUAHANG == vc.IDCUAHANG)
                        .OrderByDescending(p => p.IDVOUCHERSHOP);

                    // Thực hiện tìm kiếm nếu có chuỗi tìm kiếm
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        vouchers = (IOrderedQueryable<VOUCHERSHOP>)vouchers.Where(p => p.TENVC.Contains(searchString));
                    }

                    // Lưu trạng thái tìm kiếm hiện tại
                    currentFilter = searchString;

                    var pageNumber = page ?? 1;
                    int pageSize = size ?? 10;
                    var pagedVouchers = vouchers
                        .ToPagedList(pageNumber, pageSize);

                    return View(pagedVouchers);

                }
            }
            // Lấy thông tin voucher từ cơ sở dữ liệu
            VOUCHERSHOP voucher = new VOUCHERSHOP();

            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now.Date;

            if (voucher != null)
            {
                if (voucher.NGAYBD > currentDate)
                {
                    voucher.IDTRANGTHAIVC = 1;
                }
                else if (voucher.NGAYKT < currentDate)
                {
                    voucher.IDTRANGTHAIVC = 3;
                }
                else
                {
                    voucher.IDTRANGTHAIVC = 2;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Error");
        }

        // GET: Tao moi voucher
        public ActionResult TaoVoucher() 
        {
            VOUCHERSHOP vcs = new VOUCHERSHOP();
            return View(vcs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoVoucher(VOUCHERSHOP model)
        {
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Register");
            }

            // Lấy thông tin của người dùng trong cơ sở dữ liệu
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);

            if (nguoidung != null) // Kiểm tra xem người dùng có tồn tại hay không
            {
                // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
                var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
                {
                    model.IDCUAHANG = cuahang.IDCUAHANG;

                }
            }
            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now.Date;

            // Kiểm tra nếu ngày bắt đầu của voucher đã đến hoặc sau ngày hiện tại
            if (model.NGAYBD > currentDate)
            {
                // Đặt trạng thái của voucher là "Chưa bắt đầu"
                model.IDTRANGTHAIVC = 1;
            }
            // Kiểm tra nếu ngày kết thúc của voucher đã qua
            else if (model.NGAYKT < currentDate)
            {
                // Đặt trạng thái của voucher là "Đã kết thúc"
                model.IDTRANGTHAIVC = 3;
            }
            else
            {
                // Nếu không thuộc hai trường hợp trên, voucher đang có hiệu lực
                model.IDTRANGTHAIVC = 2;
            }
            model.DADUNG = 0;
            db.VOUCHERSHOPs.Add(model);
            db.SaveChanges();
            return RedirectToAction("QlVoucherShop");
        }

        // GET: Cap nhat voucher
        public ActionResult CapNhatVoucher(int id)
        {
            return View(db.VOUCHERSHOPs.Where(s => s.IDVOUCHERSHOP == id).FirstOrDefault());
        }

        // POST: Cap nhat voucher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatVoucher(VOUCHERSHOP vc)
        {
            var voucher = db.VOUCHERSHOPs.Find(vc.IDVOUCHERSHOP);

            voucher.TENVC = vc.TENVC;
            voucher.GIAMGIA = vc.GIAMGIA;
            voucher.GIATRITOITHIEU = vc.GIATRITOITHIEU;
            voucher.DIEUKIEN = vc.DIEUKIEN;
            voucher.NGAYBD = vc.NGAYBD;
            voucher.NGAYKT = vc.NGAYKT;
            voucher.SOLUONGSD = vc.SOLUONGSD;

            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now.Date;

            // Kiểm tra nếu ngày bắt đầu của voucher đã đến hoặc sau ngày hiện tại
            if (voucher.NGAYBD > currentDate)
            {
                // Đặt trạng thái của voucher là "Chưa bắt đầu"
                vc.IDTRANGTHAIVC = 1;
                voucher.IDTRANGTHAIVC = vc.IDTRANGTHAIVC;
            }
            // Kiểm tra nếu ngày kết thúc của voucher đã qua
            else if (voucher.NGAYKT < currentDate)
            {
                // Đặt trạng thái của voucher là "Đã kết thúc"
                vc.IDTRANGTHAIVC = 3;
                voucher.IDTRANGTHAIVC = vc.IDTRANGTHAIVC;
            }
            else
            {
                // Nếu không thuộc hai trường hợp trên, voucher đang có hiệu lực
                vc.IDTRANGTHAIVC = 2;
                voucher.IDTRANGTHAIVC = vc.IDTRANGTHAIVC;
            }

            db.SaveChanges();
            return RedirectToAction("QLVoucherShop");
        }


        // GET: Admin/Dienthoai/Delete/5
        public ActionResult Delete(int id)
        {
            var deleting = db.VOUCHERSHOPs.Find(id);

            return View(deleting);
        }

        // POST: Admin/Dienthoai/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var deleting = db.VOUCHERSHOPs.Find(id);
                db.VOUCHERSHOPs.Remove(deleting);
                db.SaveChanges();
                return RedirectToAction("QLVoucherShop");
            }
            catch
            {
                return Content("Thông tin này đã được sử dụng!");
            }
        }
    }
}