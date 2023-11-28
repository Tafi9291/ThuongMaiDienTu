using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class ShopController : Controller
    {
        TMDTEntities db = new TMDTEntities();

        // GET: Shop
        public ActionResult Shop(int id, int? page)
        {
            var cuahang = db.CUAHANGs.Find(id);
            ShopViewModel viewModel = new ShopViewModel();
            var areas = db.THANHPHOes.ToList();
            ViewBag.Areas = areas;

            var email = Session["Email"] as string;
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            
            if (cuahang != null)
            {
                viewModel.CuaHang = cuahang;

                var sanpham = db.SANPHAMs.Where(t => t.IDCUAHANG == cuahang.IDCUAHANG).OrderBy(t => t.IDSANPHAM).ToList();
                //var voucherShop = db.VOUCHERSHOPs.Where(t => t.IDCUAHANG == cuahang.IDCUAHANG).ToList();

                //ViewBag.voucherShop = voucherShop;
                //if (nguoidung != null)
                //{
                //    // Lấy danh sách voucherShop từ cơ sở dữ liệu hoặc bất kỳ nguồn dữ liệu nào khác
                //    var allVoucherShop = db.VOUCHERSHOPs.Where(t => t.IDCUAHANG == cuahang.IDCUAHANG).ToList();

                //    // Kiểm tra xem người dùng đã lưu voucher nào chưa
                //    var savedVouchers = db.VOUCHERSHOPs.Where(t => t.IDVOUCHERSHOP == nguoidung.IDVOUCHERSHOP).ToList();
                //    var availableVouchers = allVoucherShop.Where(v => !savedVouchers.Any(s => s.IDVOUCHERSHOP == v.IDVOUCHERSHOP)).ToList();
                //    ViewBag.voucherShop = availableVouchers;
                //} else
                //{
                //    var voucherShop = db.VOUCHERSHOPs.Where(t => t.IDCUAHANG == cuahang.IDCUAHANG && t.IDTRANGTHAIVC == 2).ToList();
                //    ViewBag.voucherShop = voucherShop;
                //}
                if (nguoidung != null)
                {
                    //var vouchersNotSavedByUser = db.VOUCHERSHOPs
                    //    .Where(v => !nguoidung.VOUCHERSHOPs.Any(uv => uv.IDVOUCHERSHOP == v.IDVOUCHERSHOP) && v.IDCUAHANG == cuahang.IDCUAHANG)
                    //    .ToList();
                    var vouchersSavedByUserIds = nguoidung.VOUCHERSHOPs.Select(uv => uv.IDVOUCHERSHOP);
                    var vouchersNotSavedByUser = db.VOUCHERSHOPs
                        .Where(v => !vouchersSavedByUserIds.Contains(v.IDVOUCHERSHOP) && v.IDCUAHANG == cuahang.IDCUAHANG)
                        .ToList();
                    ViewBag.voucherShop = vouchersNotSavedByUser;
                }
                else
                {
                    ViewBag.voucherShop = db.VOUCHERSHOPs.Where(t => t.IDCUAHANG == cuahang.IDCUAHANG && t.IDTRANGTHAIVC == 2).ToList();
                }

                // Số sản phẩm trên mỗi trang
                int pageSize = 10;
                // Số trang hiện tại, nếu không có thì mặc định là 1
                int pageNumber = (page ?? 1);
                // Áp dụng phân trang cho danh sách sản phẩm
                viewModel.SanPhamPagedList = sanpham.ToPagedList(pageNumber, pageSize);

            }
            else
            {
                viewModel.CuaHang = new CUAHANG();
                viewModel.SanPhamPagedList = new List<SANPHAM>().ToPagedList(1, 10); // Tạo danh sách trống nếu không có dữ liệu
                ViewBag.voucherShop = Enumerable.Empty<VOUCHERSHOP>();
            }



            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SaveVoucher(int id)
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
                // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
                //var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                //if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
                //{
                    
                //    return RedirectToAction("Shop", "Shop", new { id = id = cuahang.IDCUAHANG });
                //}
                var voucherToSave = db.VOUCHERSHOPs.FirstOrDefault(v => v.IDVOUCHERSHOP == id);
                if (voucherToSave != null)
                {
                    // Kiểm tra xem người dùng đã lưu voucher này chưa
                    var existingVoucher = nguoidung.VOUCHERSHOPs.FirstOrDefault(v => v.IDVOUCHERSHOP == voucherToSave.IDVOUCHERSHOP);
                    if (existingVoucher == null)
                    {
                        nguoidung.VOUCHERSHOPs.Add(voucherToSave);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Shop", "Shop", new { id = voucherToSave.IDCUAHANG });
                }

            }
            //if (nguoidung != null) // Kiểm tra xem người dùng có tồn tại hay không
            //{
            //    // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
            //    var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

            //    if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
            //    {
            //        // Kiểm tra xem mã voucher có tồn tại không và lưu vào cơ sở dữ liệu nếu hợp lệ
            //        var voucher = db.VOUCHERSHOPs.FirstOrDefault(v => v.IDVOUCHERSHOP == id && v.IDCUAHANG == cuahang.IDCUAHANG);
            //        if (voucher != null)
            //        {
            //            //nguoidung.IDVOUCHERSHOP = voucher.IDVOUCHERSHOP;
            //            voucher.SOLUONGSD -= 1;
            //            voucher.DADUNG += 1;
            //            db.SaveChanges();
            //            return RedirectToAction("Shop", "Shop", new { id = cuahang.IDCUAHANG }); // Chuyển hướng về trang chủ hoặc trang khác
            //        }
            //    }
            //}
            // Xử lý khi mã voucher không hợp lệ
            ModelState.AddModelError("voucherCode", "Voucher không đúng");
            return View(); // Trả về view với thông báo lỗi
        }
    }
}