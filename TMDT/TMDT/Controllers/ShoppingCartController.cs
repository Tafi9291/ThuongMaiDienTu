using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class ShoppingCartController : Controller
    {
        TMDTEntities db = new TMDTEntities();

        //private readonly Cart _cart;

        ////public ShoppingCartController()
        ////{
        ////    _cart = new Cart(HttpContext);
        ////}
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // GET: ShoppingCart
        public ActionResult AddToCart(int id)
        {
            var pro = db.SANPHAMs.SingleOrDefault(s => s.IDSANPHAM == id);

            if (pro != null)
            {
                GetCart().Add(pro, 1);

            }
            //return RedirectToAction("ShowToCart", "ShoppingCart");
            return RedirectToAction("TrangChu", "NgMua");
        }

        public ActionResult ShowToCart()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("ShowToCart", "ShoppingCart");
            }
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            Cart cart = Session["Cart"] as Cart;
            ViewBag.Message = TempData["Message"];

            return View(cart);
        }

        public PartialViewResult GioHang()
        {
            int tongsl = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                tongsl = cart.Tongsoluong();
            }
            ViewBag.infoCart = tongsl;
            return PartialView("GioHang");
        }

        public ActionResult Update_Sl(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["ID_sanpham"]);
            int sl = int.Parse(form["Soluong"]);
            cart.Update_Sl(id_pro, sl, this.ControllerContext);

            return RedirectToAction("ShowToCart", "ShoppingCart");

        }
        public ActionResult RemoveCart(int id)
        {
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            Cart cart = Session["Cart"] as Cart;
            cart.Xoasp(id);
            if (cart.Tongsoluong() == 0)
            {
                return RedirectToAction("ChuaCoHang", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("ShowToCart", "ShoppingCart");
            }

        }

        // GET: ChuaCoHang
        public ActionResult ChuaCoHang()
        {
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            return View();
        }

        public ActionResult Complete()
        {
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            return View();
        }
        public ActionResult Checkout(int id, FormCollection form)
        {
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            var email = Session["Email"] as string;
            // Kiểm tra xem giỏ hàng có sẵn hay không
            if (Session["Cart"] == null)
            {
                return RedirectToAction("ShowToCart", "ShoppingCart");
            }

            // Tạo cửa hàng cụ thể dựa trên id và lấy danh sách sản phẩm
            CUAHANG cuahang = GetCuaHangById(id);

            if (cuahang == null)
            {
                // Xử lý trường hợp id cửa hàng không hợp lệ
                return HttpNotFound(); // Hoặc xử lý theo ý bạn
            }

            // Lấy giỏ hàng từ session
            Cart cart = Session["Cart"] as Cart;

            // Truyền cửa hàng và danh sách sản phẩm vào view
            ViewBag.Message = TempData["Message"];
            ViewBag.CuaHang = cuahang;
            ViewBag.ItemsForCuaHang = cart.Items.Where(item => item.sanpham.IDCUAHANG == cuahang.IDCUAHANG).ToList();

            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null)
            {
                var cacVoucherDaLuu = nguoidung.VOUCHERSHOPs.Where(v => v.IDCUAHANG == cuahang.IDCUAHANG).ToList();
                ViewBag.UserInfo = nguoidung; // Đưa thông tin người dùng vào ViewBag
                ViewBag.VoucherShop = cacVoucherDaLuu;

                // Lấy thông tin từ TempData và gán vào ViewBag để sử dụng trong view
                ViewBag.TenVoucher = TempData["TenVoucher"];
                ViewBag.GiamGia = TempData["GiamGia"];

                TempData.Remove("TenVoucher");
                TempData.Remove("GiamGia");
            }

            return View(cart);
        }
        public CUAHANG GetCuaHangById(int id)
        {
            // Thực hiện truy vấn hoặc tìm kiếm cửa hàng dựa trên id và trả về cửa hàng cụ thể
            CUAHANG cuahang = db.CUAHANGs.FirstOrDefault(c => c.IDCUAHANG == id);

            if (cuahang != null)
            {
                // Lấy danh sách sản phẩm liên quan đến cửa hàng
                cuahang.SANPHAMs = db.SANPHAMs.Where(sp => sp.IDCUAHANG == cuahang.IDCUAHANG).ToList();
            }

            return cuahang;
        }

        //public ActionResult ProcessVoucher(int voucherId, double totalCartAfAll)
        //{
        //    // Lấy thông tin voucher từ CSDL sử dụng voucherId
        //    var selectedVoucher = db.VOUCHERSHOPs.FirstOrDefault(v => v.IDVOUCHERSHOP == voucherId);
        //    if (selectedVoucher != null)
        //    {
        //        var voucherDiscount = selectedVoucher?.GIAMGIA ?? 0;
        //        var totalDiscountedPrice = totalCartAfAll - voucherDiscount;
        //        // Format chuỗi voucherDiscount
        //        var formattedDiscount = string.Format("{0:#,##0 đ}", voucherDiscount);
        //        var formattedTotalDiscountedPrice = string.Format("{0:#,##0 đ}", totalDiscountedPrice);

        //        // Trả về thông tin voucher cho client
        //        return Json(new
        //        {
        //            success = true,
        //            voucherName = selectedVoucher.TENVC,
        //            voucherDiscount = formattedDiscount,
        //            totalDiscountedPrice = formattedTotalDiscountedPrice,
        //            totalDiscounted = totalDiscountedPrice
        //        });
        //    }
        //    else
        //    {
        //        return Json(new { success = false });
        //    }
        //}
        [HttpPost]
        public ActionResult AddVoucher(int id)
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
                var voucher = db.VOUCHERSHOPs.SingleOrDefault(v => v.IDVOUCHERSHOP == id);
                if (voucher != null)
                {
                    // Lấy thông tin cần thiết từ voucher như tên và giảm giá
                    var tenVoucher = voucher.TENVC;
                    var giamGia = voucher.GIAMGIA;
                    var idvoucher = voucher.IDVOUCHERSHOP;

                    var formattedGiamGia = string.Format("{0:#,##0}đ", giamGia);

                    // Ví dụ: Trả về tên và giảm giá để sử dụng trong view
                    TempData["TenVoucher"] = tenVoucher;
                    TempData["GiamGia"] = formattedGiamGia;

                    Session["GiamGia"] = formattedGiamGia;
                    // Trả về view hoặc thực hiện các hành động tiếp theo tùy vào logic của bạn
                    return RedirectToAction("Checkout", "ShoppingCart", new {id = voucher.IDCUAHANG }); // Thay bằng tên view cần trả về nếu cần
                }
                else
                {
                }

            }
            return View("Error", "Home");
        }

        [HttpPost]
        public ActionResult Checkout(FormCollection form, int ShopId)
        {
            
            CUAHANG cuahang = GetCuaHangById(ShopId);
            try
            {
                // Lấy thông tin khách hàng từ session
                var email = Session["Email"] as string;
                if (Session["Email"] == null)
                {
                    // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                    return RedirectToAction("DangNhap", "Register");
                }
                else
                {
                    Cart cart = Session["Cart"] as Cart;
                    //var shop = cart.Items.FirstOrDefault()?.sanpham?.CUAHANG;
                    var shop = cart.Items.Where(s => s.sanpham.CUAHANG.IDCUAHANG == ShopId);
                    // Lấy thông tin khách hàng từ CSDL
                    var khachHang = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
                    var donHang = new DONHANG();

                    //int cuahangId = cart.Items.FirstOrDefault()?.sanpham?.CUAHANG?.IDCUAHANG ?? 0;

                    // Thêm thông tin khách hàng vào đơn hàng
                    donHang.IDND = khachHang.IDND;
                    donHang.NGUOINHAN = (form["Hovaten"]);
                    // Thêm thông tin cửa hàng vào đơn hàng
                    donHang.IDCUAHANG = ShopId; // ID của cửa hàng
                    donHang.DIACHI = (form["Diachi"]);
                    donHang.SDT = (form["SDT"]);
                    donHang.NGAYTAO = DateTime.Now;
                    
                    //// Lấy danh sách sản phẩm trong giỏ hàng thuộc cùng một cửa hàng
                    var productsInShop = cart.Items.Where(item => item.sanpham?.CUAHANG?.IDCUAHANG == ShopId);

                    int shipPrice = 26000;
                    int? thanhTienTheoShop = productsInShop.Sum(item => item.sanpham?.GIAGIAM * item.soluong + shipPrice);

                    var giamGiaString = Session["GiamGia"] as string;
                    double giamGiaValue;
                    if (giamGiaString != null)
                    {
                        if (double.TryParse(giamGiaString.Replace("đ", "").Replace(",", "").Trim(), out giamGiaValue))
                        {
                            double thanhtienApDungVC = (int)thanhTienTheoShop - giamGiaValue;
                            donHang.THANHTIEN = (int)thanhtienApDungVC;
                        }
                    }
                    else
                    {
                        donHang.THANHTIEN = thanhTienTheoShop; // Sử dụng thanhTienTheoShop nếu giảm giá không tồn tại
                    }

                    var trangThai = db.TRANGTHAIDHs.SingleOrDefault(tt => tt.IDTRANGTHAIDH == 1);

                    donHang.IDTRANGTHAIDH = trangThai.IDTRANGTHAIDH;
                    var ptthanhtoan = db.PTTHANHTOANs.SingleOrDefault(tt => tt.IDPTTHANHTOAN == 1);
                    donHang.IDPTTHANHTOAN = ptthanhtoan.IDPTTHANHTOAN;
                    var trangthaixemdon = db.TRANGTHAIXEMDONHANGs.SingleOrDefault(tt => tt.IDTRANGTHAIXDH == 1);
                    donHang.IDTRANGTHAIXDH = trangthaixemdon.IDTRANGTHAIXDH;

                    db.DONHANGs.Add(donHang);

                    foreach (var item in productsInShop)
                    {
                        CTDONHANG chiTietDonHang = new CTDONHANG();

                        chiTietDonHang.IDDONHANG = donHang.IDDONHANG;
                        chiTietDonHang.IDSANPHAM = item.sanpham.IDSANPHAM;
                        chiTietDonHang.SOLUONGMUA = item.soluong;
                        chiTietDonHang.DONGIA = item.sanpham.GIAGIAM;
                        chiTietDonHang.TONGTIEN = (item.soluong * item.sanpham.GIAGIAM);

                        db.CTDONHANGs.Add(chiTietDonHang);

                        foreach (var p in db.SANPHAMs.Where(s => s.IDSANPHAM == chiTietDonHang.IDSANPHAM))
                        {
                            var soluongtonmoi = p.SOLUONGTON - item.soluong;
                            p.SOLUONGTON = soluongtonmoi;
                        }
                    }

                    
                    db.SaveChanges();
                    cart.Xoagh(ShopId);
                    // Hiển thị thông báo đặt hàng thành công
                    ViewBag.ThongBao = "Đặt hàng thành công!";
                    Session.Remove("GiamGia");
                    return RedirectToAction("Complete", new { id = donHang.IDDONHANG });

                }
            }
            catch
            {
                return View();
            }

        }
    }
}