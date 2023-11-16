using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
namespace TMDT.Areas.Admin.Controllers
{
    public class QLNDController : Controller
    {
        TMDTEntities db=new TMDTEntities();
        // GET: Admin/QLND
        public ActionResult QLNgMua(int? size, int? page, string currentFilter, string searchString)
        {
            // Kiểm tra quyền truy cập theo CHUCVUID

            var email = Session["EmailAD"] as string;
            var admin = db.ADMINs.FirstOrDefault(c => c.EMAIL == email);
            if (admin != null && (admin.IDCHUCVU == 1 || admin.IDCHUCVU == 3))
            {
                var thongtin = new List<NGUOIDUNG>();
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    //lấy ds tên nv theo tu khóa tim kiếm
                    thongtin = db.NGUOIDUNGs.Where(n => n.TENND.Contains(searchString)).ToList();
                    
                }
                else
                {
                    //lấy ds tên nv theo bảng NV
                    thongtin = db.NGUOIDUNGs.ToList();
                }
                ViewBag.CurrenFilter = searchString;
                // 1. Tạo list pageSize để người dùng có thể chọn xem để phân trang
                // Bạn có thể thêm bớt tùy ý 
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "5", Value = "5" });
                items.Add(new SelectListItem { Text = "10", Value = "10" });
                items.Add(new SelectListItem { Text = "20", Value = "20" });


                // 1.1. Giữ trạng thái kích thước trang được chọn trên DropDownList
                foreach (var item in items)
                {
                    if (item.Value == size.ToString()) item.Selected = true;
                }

                // 1.2. Tạo các biến ViewBag
                ViewBag.size = items; // ViewBag DropDownList
                ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại

                // 2. Nếu page = null thì đặt lại là 1.
                page = page ?? 1; //if (page == null) page = 1;

                // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
                // theo LinkID mới có thể phân trang.
                thongtin = thongtin.OrderBy(n => n.IDND).ToList();

                // 4. Tạo kích thước trang (pageSize), mặc định là 5.
                int pageSize = (size ?? 5);

                // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
                // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
                int pageNumber = (page ?? 1);
                //4.2 Lấy tổng số record chia cho kích thuốc để biết bao nhiêu trang
                int checkTotal = (int)(thongtin.ToList().Count / pageSize) + 1;
                //Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tống số trang
                if (pageNumber > checkTotal) pageNumber = checkTotal;

                // 5. Trả về các Link được phân trang theo kích thước và số trang.
                return View(thongtin.ToPagedList(pageNumber, pageSize));
            }
            // Người dùng không có quyền truy cập, chuyển hướng đến trang lỗi hoặc xử lý khác
            return RedirectToAction("Khongcoquyen");
        }
      
        // GET
        public ActionResult QLNgBan(int? size, int? page, string currentFilter, string searchString)
        {
            // Kiểm tra quyền truy cập theo CHUCVUID

            var email = Session["EmailAD"] as string;
            var admin = db.ADMINs.FirstOrDefault(c => c.EMAIL == email);
            if (admin != null && (admin.IDCHUCVU == 1 || admin.IDCHUCVU == 3))
            {
                var thongtin = new List<CUAHANG>();
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    //lấy ds tên nv theo tu khóa tim kiếm
                    thongtin = db.CUAHANGs.Where(n => n.TENCH.Contains(searchString)).ToList();

                }
                else
                {
                    //lấy ds tên nv theo bảng NV
                    thongtin = db.CUAHANGs.ToList();
                }
                ViewBag.CurrenFilter = searchString;
                // 1. Tạo list pageSize để người dùng có thể chọn xem để phân trang
                // Bạn có thể thêm bớt tùy ý 
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "5", Value = "5" });
                items.Add(new SelectListItem { Text = "10", Value = "10" });
                items.Add(new SelectListItem { Text = "20", Value = "20" });


                // 1.1. Giữ trạng thái kích thước trang được chọn trên DropDownList
                foreach (var item in items)
                {
                    if (item.Value == size.ToString()) item.Selected = true;
                }

                // 1.2. Tạo các biến ViewBag
                ViewBag.size = items; // ViewBag DropDownList
                ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại

                // 2. Nếu page = null thì đặt lại là 1.
                page = page ?? 1; //if (page == null) page = 1;

                // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
                // theo LinkID mới có thể phân trang.
                thongtin = thongtin.OrderBy(n => n.IDCUAHANG).ToList();

                // 4. Tạo kích thước trang (pageSize), mặc định là 5.
                int pageSize = (size ?? 5);

                // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
                // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
                int pageNumber = (page ?? 1);
                //4.2 Lấy tổng số record chia cho kích thuốc để biết bao nhiêu trang
                int checkTotal = (int)(thongtin.ToList().Count / pageSize) + 1;
                //Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tống số trang
                if (pageNumber > checkTotal) pageNumber = checkTotal;

                // 5. Trả về các Link được phân trang theo kích thước và số trang.
                return View(thongtin.ToPagedList(pageNumber, pageSize));
            }
            // Người dùng không có quyền truy cập, chuyển hướng đến trang lỗi hoặc xử lý khác
            return RedirectToAction("Khongcoquyen");
        }
        public ActionResult Khongcoquyen()
        {
            // Thực hiện các logic xử lý hoặc trả về view tương ứng
            return View();
        }
    }
}