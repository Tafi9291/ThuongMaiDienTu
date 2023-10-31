using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class DonHangController : Controller
    {
        TMDTEntities db = new TMDTEntities();

        // GET: DonHang
        public ActionResult QLDonHang(int? size, int? page, string currentFilter, string searchString)
        {
            SANPHAM sp = new SANPHAM();
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
                    sp.IDCUAHANG = cuahang.IDCUAHANG;
                    // Lấy danh sách sản phẩm theo IDCUAHANG
                    var orders = db.DONHANGs
                        .Where(p => p.IDCUAHANG == sp.IDCUAHANG)
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
            }
            ////const int pageSize = 10;
            ////var pageNumber = (page ?? 1);
            ////var products = db.SANPHAMs.OrderBy(p => p.TENSP).ToList().ToPagedList(pageNumber, pageSize);
            //return RedirectToAction("");
            return RedirectToAction("ErrorPage"); // Xử lý khi không tìm thấy cửa hàng hoặc người dùng
        }

        public ActionResult SelectCateTT()
        {
            TRANGTHAIDH se_cate = new TRANGTHAIDH();

            se_cate.ListCateTT = db.TRANGTHAIDHs.ToList<TRANGTHAIDH>();
            return PartialView(se_cate);
        }
        public ActionResult SelectCatePTTT()
        {
            PTTHANHTOAN se_cate = new PTTHANHTOAN();
            se_cate.ListCatePTTT = db.PTTHANHTOANs.ToList<PTTHANHTOAN>();
            return PartialView(se_cate);
        }
        // GET: Edit DonHang
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
                sua.NGUOIDUNG.TENND = model.NGUOIDUNG.TENND;
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

        // GET: Detail DonHang
        public ActionResult CTDonHang()
        {
            return View();
        }
    }
}