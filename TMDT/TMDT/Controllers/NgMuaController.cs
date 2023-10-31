using PagedList;
using System;
using System.Collections;
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
        public ActionResult TrangChu(int? page)
        {
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            var products = db.SANPHAMs.OrderBy(p => p.TENSP).ToPagedList(pageNumber, pageSize);
            return View(products);
        }

        public ActionResult Loai()
        {
            var loaisp = db.LOAISPs.ToList();
            return PartialView(loaisp);
        }

        public ActionResult LoaiSanPham(int id, int page = 1)
        {
            var dienthoais = db.SANPHAMs.Where(d => d.IDLOAISP == id).ToList().ToPagedList(page, 10); // 10 là số phần tử trên mỗi trang
            return View(dienthoais);
        }

        public ActionResult Search(int? page, string searching)
        {
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            return View(db.SANPHAMs.Where(x => x.TENSP.StartsWith(searching) || x.TENSP == null).ToList().ToPagedList(pageNumber, pageSize));

        }

        // GET: NgMua/SanPham/Detail
        public ActionResult ChiTietSanPham(int id)
        {
            var sanpham = db.SANPHAMs.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            var ctDienThoai = db.CTDIENTHOAIs.FirstOrDefault(t => t.IDSANPHAM == sanpham.IDSANPHAM);
            var ctThoiTrang = db.CTTHOITRANGs.FirstOrDefault(t => t.IDSANPHAM == sanpham.IDSANPHAM);
            var ctGiay = db.CTGIAYs.FirstOrDefault(t => t.IDSANPHAM == sanpham.IDSANPHAM);

            ViewBag.ChiTietDienThoai = ctDienThoai;
            ViewBag.ChiTietThoiTrang = ctThoiTrang;
            ViewBag.ChiTietGiay = ctGiay;

            var email = Session["Email"] as string;
            // Check if user already reviewed this product
            var existingReview = db.DANHGIASPs.FirstOrDefault(r => r.IDND.ToString() == email && r.IDSANPHAM == id);
            if (existingReview != null)
            {
                ViewBag.CanReview = false;
                ViewBag.UserReview = existingReview;
            }
            else
            {
                ViewBag.CanReview = true;
            }

            var cuaHang = db.CUAHANGs.FirstOrDefault(c => c.IDCUAHANG == sanpham.IDCUAHANG);
            ViewBag.CuaHang = cuaHang;

            if (cuaHang != null)
            {
                var voucherShop = db.VOUCHERSHOPs.Where(v => v.IDCUAHANG == cuaHang.IDCUAHANG).ToList();
                ViewBag.VoucherShop = voucherShop;

                var productShop = db.SANPHAMs.Where(v => v.IDCUAHANG == cuaHang.IDCUAHANG && v.IDSANPHAM != id).ToList();
                ViewBag.ProductShop = productShop;
            }
            else { 
                ViewBag.VoucherShop = Enumerable.Empty<VOUCHERSHOP>();
                ViewBag.ProductShop = Enumerable.Empty<SANPHAM>();
            }


            return View(sanpham);
        }

        public ActionResult DonMua(int? trangThaiId)
        {
            // Lấy thông tin khách hàng từ session
            var email = Session["Email"] as string;
            var hinh = Session["Hinh"] as string;
            if (Session["Email"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Register");
            }
            else
            {
                // Lấy danh sách đơn hàng của khách hàng từ CSDL
                var khachHang = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
                var donHangs = db.DONHANGs.Where(dh => dh.IDND == khachHang.IDND);

                // Lọc danh sách đơn hàng theo trạng thái được chọn (nếu có)
                if (trangThaiId.HasValue)
                {
                    donHangs = donHangs.Where(dh => dh.IDTRANGTHAIDH == trangThaiId.Value);
                }

                return View(donHangs.ToList());
            }
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var donHang = db.DONHANGs.SingleOrDefault(dh => dh.IDDONHANG == id);
            var chiTietDonHangs = db.CTDONHANGs.Where(ct => ct.IDDONHANG == id).ToList();
            ViewBag.DonHang = donHang;
            return View(chiTietDonHangs);
        }

        public ActionResult Khongduochuy()
        {
            // Thực hiện các logic xử lý hoặc trả về view tương ứng
            return View();
        }

        public ActionResult Huydonhang(int id)
        {
            var donHang = db.DONHANGs.SingleOrDefault(dh => dh.IDDONHANG == id);

            // Kiểm tra nếu IDTRANGTHAIDH là 5 hoặc 6, không cho phép hủy đơn hàng
            if (donHang.IDTRANGTHAIDH == 4 || donHang.IDTRANGTHAIDH == 7)
            {
                // Redirect hoặc hiển thị thông báo lỗi cho khách hàng
                return RedirectToAction("Khongduochuy", "NgMua");
            }

            // Kiểm tra nếu TRANGTHAIID là từ 1 đến 4, chuyển TRANGTHAIID thành 7
            if (donHang.IDTRANGTHAIDH >= 1 && donHang.IDTRANGTHAIDH < 3)
            {
                donHang.IDTRANGTHAIDH = 8;
                db.SaveChanges();
            }

            var chiTietDonHangs = db.CTDONHANGs.Where(ct => ct.IDDONHANG == id).ToList();
            ViewBag.DonHang = donHang;

            return View(chiTietDonHangs);

        }
    }
}