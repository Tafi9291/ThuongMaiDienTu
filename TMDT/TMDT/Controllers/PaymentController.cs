using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
using System.Web.Helpers;
using System.Web.UI.WebControls.WebParts;
using TMDT.Payment;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace TMDT.Controllers
{
    public class PaymentController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        // GET: Payment
        public ActionResult Payment(FormCollection form, int ShopId)
        {
            TempData["ShopIdForConfirmation"] = ShopId;

            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];


            // Lấy thông tin khách hàng từ session
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Register");
            }
            else
            {

                Cart cart = Session["Cart"] as Cart;
                PayLib pay = new PayLib();

                pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
                pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                var giamGiaString = Session["GiamGia"] as string;
                double giamGiaValue;
                if (giamGiaString != null)
                {
                    if (double.TryParse(giamGiaString.Replace("đ", "").Replace(",", "").Trim(), out giamGiaValue))
                    {
                        double thanhtienApDungVC = (int)cart.TongtienCheckout() - giamGiaValue;
                        pay.AddRequestData("vnp_Amount", (thanhtienApDungVC * 100).ToString());
                    }
                }
                else
                {
                    pay.AddRequestData("vnp_Amount", (cart.TongtienCheckout() * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                }
                pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
                pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
                pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
                pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

                string paymentUrl = pay.CreateRequestUrl(url, hashSecret);


                return Redirect(paymentUrl);

            }

        }
        private CUAHANG GetCuaHangById(int id)
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
        public ActionResult PaymentConfirm(FormCollection form)
        {
            TMDTEntities db = new TMDTEntities();

            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;
            // Continue with the rest of your existing code...
            int ShopId = (int)TempData["ShopIdForConfirmation"];

            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
                var vnpayData = Request.QueryString;
                var email = Session["Email"] as string;
                PayLib pay = new PayLib();
                Cart cart = Session["Cart"] as Cart;

                // Extract query string data
                foreach (string key in vnpayData)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(key, vnpayData[key]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode");
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"];

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {

                        // Check if the user is logged in
                        var email1 = Session["Email"] as string;
                        if (Session["Email"] == null)
                        {
                            // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                            return RedirectToAction("DangNhap", "Register");
                        }
                        else
                        {
                            Cart cart1 = Session["Cart"] as Cart;
                            //var shop = cart.Items.FirstOrDefault()?.sanpham?.CUAHANG;
                            var shop = cart1.Items.Where(s => s.sanpham.CUAHANG.IDCUAHANG == ShopId);
                            // Lấy thông tin khách hàng từ CSDL
                            var khachHang = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email1);
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
                            //donHang.THANHTIEN = (int?)cart.Tongtien().Values.Sum();
                            // Lấy danh sách sản phẩm trong giỏ hàng thuộc cùng một cửa hàng
                            var productsInShop = cart1.Items.Where(item => item.sanpham?.CUAHANG?.IDCUAHANG == ShopId);

                            // Tính tổng giá trị của các sản phẩm thuộc cùng một cửa hàng
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
                            // Lấy trạng thái đơn hàng mặc định (ví dụ: 1 - Chờ xử lý)
                            var trangThai = db.TRANGTHAIDHs.SingleOrDefault(tt => tt.IDTRANGTHAIDH == 1);

                            // Thêm thông tin trạng thái vào đơn hàng
                            donHang.IDTRANGTHAIDH = trangThai.IDTRANGTHAIDH;
                            var ptthanhtoan = db.PTTHANHTOANs.SingleOrDefault(tt => tt.IDPTTHANHTOAN == 2);
                            donHang.IDPTTHANHTOAN = ptthanhtoan.IDPTTHANHTOAN;
                            var trangthaixemdon = db.TRANGTHAIXEMDONHANGs.SingleOrDefault(tt => tt.IDTRANGTHAIXDH == 1);
                            donHang.IDTRANGTHAIXDH = trangthaixemdon.IDTRANGTHAIXDH;
                            // Lưu đơn hàng vào CSDL
                            db.DONHANGs.Add(donHang);


                            // Thêm sản phẩm vào chi tiết đơn hàng


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

                            // Xóa thông tin giỏ hàng theo cửa hàng đã bấm thanh toán
                            cart1.Xoagh(ShopId);

                            // Payment successful message
                            ViewBag.Message = "Payment successful for order " + orderId + " | Transaction ID: " + vnpayTranId;
                        }
                    }
                    else
                    {
                        // Payment unsuccessful message with error code
                        ViewBag.Message = "Error processing order " + orderId + " | Transaction ID: " + vnpayTranId + " | Error Code: " + vnp_ResponseCode;
                    }

                }
                else
                {
                    // Error message for invalid signature
                    ViewBag.Message = "Error processing the order. Invalid signature.";
                }
            }
            return View();
        }
    }
}
    