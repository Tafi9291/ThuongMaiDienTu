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
    }
}