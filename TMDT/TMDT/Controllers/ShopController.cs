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

            if (cuahang != null)
            {
                viewModel.CuaHang = cuahang;

                var sanpham = db.SANPHAMs.Where(t => t.IDCUAHANG == cuahang.IDCUAHANG).OrderBy(t => t.IDSANPHAM).ToList();
                var voucherShop = db.VOUCHERSHOPs.Where(t => t.IDCUAHANG == cuahang.IDCUAHANG).ToList();
                
                ViewBag.voucherShop = voucherShop;

                // Số sản phẩm trên mỗi trang
                int pageSize = 10;
                // Số trang hiện tại, nếu không có thì mặc định là 1
                int pageNumber = (page ?? 1);
                // Áp dụng phân trang cho danh sách sản phẩm
                viewModel.SanPhamPagedList = sanpham.ToPagedList(pageNumber, pageSize);

            } else
            {
                viewModel.CuaHang = new CUAHANG();
                viewModel.SanPhamPagedList = new List<SANPHAM>().ToPagedList(1, 10); // Tạo danh sách trống nếu không có dữ liệu
                ViewBag.voucherShop = Enumerable.Empty<VOUCHERSHOP>();
            }


            return View(viewModel);
        }
    }
}