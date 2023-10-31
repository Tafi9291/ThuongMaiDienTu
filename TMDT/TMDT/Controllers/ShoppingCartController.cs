using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class ShoppingCartController : Controller
    {
        TMDTEntities db = new TMDTEntities();
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
            return View();
        }

        public ActionResult Complete()
        {
            return View();
        }

        public ActionResult Checkout(FormCollection form)
        {
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
                    // Lấy thông tin khách hàng từ CSDL
                    var khachHang = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
                    var donHang = new DONHANG();

                    // Thêm thông tin khách hàng vào đơn hàng
                    donHang.IDND = khachHang.IDND;
                    donHang.NGUOINHAN = (form["Hovaten"]);

                    donHang.DIACHI = (form["Diachi"]);
                    donHang.SDT = (form["SDT"]);
                    donHang.NGAYTAO = DateTime.Now;
                    donHang.THANHTIEN = (int?)cart.Tongtien().Values.Sum();
                    // Lấy trạng thái đơn hàng mặc định (ví dụ: 1 - Chờ xử lý)
                    var trangThai = db.TRANGTHAIDHs.SingleOrDefault(tt => tt.IDTRANGTHAIDH == 1);

                    // Thêm thông tin trạng thái vào đơn hàng
                    donHang.IDTRANGTHAIDH = trangThai.IDTRANGTHAIDH;
                    var ptthanhtoan = db.PTTHANHTOANs.SingleOrDefault(tt => tt.IDPTTHANHTOAN == 1);
                    donHang.IDPTTHANHTOAN = ptthanhtoan.IDPTTHANHTOAN;
                    // Lưu đơn hàng vào CSDL
                    db.DONHANGs.Add(donHang);


                    // Thêm sản phẩm vào chi tiết đơn hàng


                    foreach (var item in cart.Items)
                    {
                        CTDONHANG chiTietDonHang = new CTDONHANG();


                        chiTietDonHang.IDDONHANG = donHang.IDDONHANG;
                        chiTietDonHang.IDSANPHAM = item.sanpham.IDSANPHAM;
                        chiTietDonHang.SOLUONGMUA = item.soluong;


                        chiTietDonHang.TONGTIEN = (item.soluong * item.sanpham.GIAGIAM);

                        db.CTDONHANGs.Add(chiTietDonHang);
                        foreach (var p in db.SANPHAMs.Where(s => s.IDSANPHAM == chiTietDonHang.IDSANPHAM))
                        {
                            var soluongtonmoi = p.SOLUONGTON - item.soluong;
                            p.SOLUONGTON = soluongtonmoi;
                        }
                    }

                    db.SaveChanges();

                    // Xóa thông tin giỏ hàng
                    cart.Xoagh();

                    // Hiển thị thông báo đặt hàng thành công
                    ViewBag.ThongBao = "Đặt hàng thành công!";

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