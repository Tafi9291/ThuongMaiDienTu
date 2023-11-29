using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public ActionResult TrangChu(int? page, string sortOrder, string selectedArea)
        {
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            // Lấy danh sách sản phẩm theo IDPHEDUYET
            var productsQuery = db.SANPHAMs.Where(p => p.IDPHEDUYET == 2);

            LoadAreasToViewBag();

            // Lọc sản phẩm theo khu vực (tỉnh thành)
            if (!string.IsNullOrEmpty(selectedArea))
            {
                productsQuery = productsQuery.Where(p => p.CUAHANG.THANHPHO.TENTHANHPHO == selectedArea);
            }

            // Sắp xếp theo giá
            switch (sortOrder)
            {
                case "giathapdencao":
                    productsQuery = productsQuery.OrderBy(p => p.GIAGIAM); // Sắp xếp từ thấp đến cao
                    break;
                case "giacaodenthap":
                    productsQuery = productsQuery.OrderByDescending(p => p.GIAGIAM); // Sắp xếp từ cao đến thấp
                    break;
                default:
                    productsQuery = productsQuery.OrderBy(p => p.TENSP); // Mặc định sắp xếp theo tên sản phẩm
                    break;
            }

            var products = productsQuery.ToPagedList(pageNumber, pageSize);
            return View(products);
        }

        public ActionResult Loai()
        {
            var loaisp = db.LOAISPs.ToList();
            return PartialView(loaisp);
        }

        public ActionResult LoaiSanPham(int id, int page = 1, string sortOrder = "", string selectedArea = "")
        {
            const int pageSize = 10;
            var pageNumber = page;

            var productsQuery = db.SANPHAMs.Where(d => d.IDLOAISP == id && d.IDPHEDUYET == 2);

            LoadAreasToViewBag();

            // Lọc sản phẩm theo khu vực (tỉnh thành)
            if (!string.IsNullOrEmpty(selectedArea))
            {
                productsQuery = productsQuery.Where(p => p.CUAHANG.THANHPHO.TENTHANHPHO == selectedArea);
            }

            // Sắp xếp sản phẩm
            switch (sortOrder)
            {
                case "giathapdencao":
                    productsQuery = productsQuery.OrderBy(p => p.GIAGIAM);
                    break;
                case "giacaodenthap":
                    productsQuery = productsQuery.OrderByDescending(p => p.GIAGIAM);
                    break;
                default:
                    productsQuery = productsQuery.OrderBy(p => p.TENSP);
                    break;
            }

            var products = productsQuery.ToPagedList(pageNumber, pageSize);
            return View(products);
        }

        private void LoadAreasToViewBag()
        {
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
        }

        public ActionResult Search( int? page, string searching, string selectedArea = "")
        {
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            var productsQuery = db.SANPHAMs.Where(d => d.IDPHEDUYET == 2);
            LoadAreasToViewBag();

            // Lọc sản phẩm theo khu vực (tỉnh thành)
            if (!string.IsNullOrEmpty(selectedArea))
            {
                productsQuery = productsQuery.Where(p => p.CUAHANG.THANHPHO.TENTHANHPHO == selectedArea);
            }

            // Tìm kiếm sản phẩm theo tên và IDPHEDUYET = 2 (đã được phê duyệt)
            var filteredProducts = db.SANPHAMs
                .Where(x => (x.TENSP.Contains(searching) || x.TENSP == null) && x.IDPHEDUYET == 2);

            // Nếu đã có điều kiện lọc theo khu vực, áp dụng điều kiện này vào sản phẩm đã lọc
            if (productsQuery.Any())
            {
                filteredProducts = filteredProducts.Where(p => productsQuery.Any(pr => pr.IDSANPHAM == p.IDSANPHAM));
            }

            return View(filteredProducts.ToList().ToPagedList(pageNumber, pageSize));

        }

        // GET: NgMua/SanPham/Detail
        public ActionResult ChiTietSanPham(int id)
        {
            LoadAreasToViewBag();
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

                var productShop = db.SANPHAMs.Where(v => v.IDCUAHANG == cuaHang.IDCUAHANG && v.IDSANPHAM != id && v.IDPHEDUYET == 2).ToList();
                ViewBag.ProductShop = productShop;
            }
            else { 
                ViewBag.VoucherShop = Enumerable.Empty<VOUCHERSHOP>();
                ViewBag.ProductShop = Enumerable.Empty<SANPHAM>();
            }
            
            // Kiểm tra xem người dùng đã thêm sản phẩm vào mục yêu thích chưa
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null)
            {
                var productToFav = db.SANPHAMs.FirstOrDefault(v => v.IDSANPHAM == id);
                if (productToFav != null)
                {
                    var existingProductFav = nguoidung.SANPHAMs.FirstOrDefault(v => v.IDSANPHAM == sanpham.IDSANPHAM);
                    ViewBag.ExistingProduct = existingProductFav;
                }
                var productsShopSuggest = db.SANPHAMs.FirstOrDefault(s => s.IDLOAISP == sanpham.IDLOAISP && s.IDSANPHAM != sanpham.IDSANPHAM && s.IDPHEDUYET == 2);
                var shopSuggest = db.CUAHANGs
                    .Where(c => c.IDTP == nguoidung.IDTP && c.IDCUAHANG != cuaHang.IDCUAHANG) // Điều kiện vị trí người dùng và cửa hàng bằng nhau
                    .Where(c => c.SANPHAMs.Any(p => p.IDLOAISP == sanpham.IDLOAISP)) // Cửa hàng có sản phẩm cùng loại
                    .ToList(); // Chuyển kết quả thành danh sách
                ViewBag.ShopSuggest = shopSuggest;
            }

            // Các sản phẩm liên quan đến sản phẩm người dùng đang xem
            var productsSuggest = db.SANPHAMs.Where(s => s.IDLOAISP == sanpham.IDLOAISP && s.IDSANPHAM != sanpham.IDSANPHAM && s.IDPHEDUYET == 2);
            ViewBag.ProductsSuggest = productsSuggest;

            


            return View(sanpham);
        }

        [HttpPost]
        public ActionResult AddReview(int id, string review)
        {
            // Check if user is logged in
            var email = Session["Email"] as string;
            if (Session["Email"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Register");
            }
            // Get logged in user id


            // Check if user already reviewed this product
            var existingReview = db.DANHGIASPs.FirstOrDefault(r => r.IDND.ToString() == email && r.IDSANPHAM == id);

            if (existingReview != null)
            {
                ModelState.AddModelError("", "Bạn đã đánh giá sản phẩm này rồi!");
            }
            else
            {
                // Add new review
                var khachHang = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
                var newReview = new DANHGIASP();


                newReview.IDND = khachHang.IDND;
                newReview.IDSANPHAM = id;
                newReview.DANHGIA = review;
                newReview.NGAYTAO = DateTime.Now;


                db.DANHGIASPs.Add(newReview);
                db.SaveChanges();
            }

            // Redirect to product details page
            return RedirectToAction("ChiTietSanPham", new { id = id });
        }

        public ActionResult DonMua(int? trangThaiId)
        {
            LoadAreasToViewBag();
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
            LoadAreasToViewBag();
            var donHang = db.DONHANGs.SingleOrDefault(dh => dh.IDDONHANG == id);
            var chiTietDonHangs = db.CTDONHANGs.Where(ct => ct.IDDONHANG == id).ToList();
            ViewBag.DonHang = donHang;
            return View(chiTietDonHangs);
        }

        public ActionResult Khongduochuy()
        {
            LoadAreasToViewBag();
            // Thực hiện các logic xử lý hoặc trả về view tương ứng
            return View();
        }

        public ActionResult Huydonhang(int id)
        {
            var donHang = db.DONHANGs.SingleOrDefault(dh => dh.IDDONHANG == id);
            LoadAreasToViewBag();
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

        // GET
        public ActionResult YourVoucher()
        {
            LoadAreasToViewBag();
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Register");
            }
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null)
            {
                var cacVoucherDaLuu = nguoidung.VOUCHERSHOPs.ToList();
                ViewBag.UserInfo = nguoidung; // Đưa thông tin người dùng vào ViewBag
                return View(cacVoucherDaLuu);
            }
            return View();
        }

        public ActionResult Favorites()
        {
            LoadAreasToViewBag();
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Register");
            }
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null)
            {
                var productFavSaved = nguoidung.SANPHAMs.ToList();
                int tongsl = productFavSaved.Count;
                ViewBag.UserInfo = nguoidung;
                ViewBag.Quantity = tongsl;
                return View(productFavSaved);
            }
            return View();
        }


        [HttpPost]
        public ActionResult AddFavorites(int id)
        {
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Register");
            }
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null)
            {
                var productToFav = db.SANPHAMs.FirstOrDefault(v => v.IDSANPHAM == id);
                if (productToFav != null)
                {
                    // Kiểm tra xem người dùng đã lưu voucher này chưa
                    var existingProductFav = nguoidung.SANPHAMs.FirstOrDefault(v => v.IDSANPHAM == productToFav.IDSANPHAM);
                    if (existingProductFav == null)
                    {
                        nguoidung.SANPHAMs.Add(productToFav);
                        db.SaveChanges();
                    }
                    else
                    {
                        nguoidung.SANPHAMs.Remove(existingProductFav);
                        db.SaveChanges();
                    }
                    return RedirectToAction("ChiTietSanPham", "NgMua", new { id = productToFav.IDSANPHAM });
                }
            }
            ModelState.AddModelError("productCode", "Sản phẩm không đúng");
            return View(); // Trả về view với thông báo lỗi
        }
    }
}