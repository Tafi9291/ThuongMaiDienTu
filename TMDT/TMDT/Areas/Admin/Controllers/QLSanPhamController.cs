using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
namespace TMDT.Areas.Admin.Controllers
{
    public class QLSanPhamController : Controller
    {
        TMDTEntities db=new TMDTEntities();
        // GET: Admin/QLSanPham
        public ActionResult QLSanPham(int? size, int? page, string currentFilter, string searchString)
        {
            // Kiểm tra quyền truy cập theo CHUCVUID

            var email = Session["Email"] as string;
            var admin = db.ADMINs.FirstOrDefault(c => c.EMAIL == email);
            if (admin != null && (admin.IDCHUCVU == 1 || admin.IDCHUCVU == 3))
            {
                var thongtin = new List<SANPHAM>();
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
                    thongtin = db.SANPHAMs.Where(n => n.TENSP.Contains(searchString)).ToList();

                }
                else
                {
                    //lấy ds tên nv theo bảng NV
                    thongtin = db.SANPHAMs.ToList();
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
                thongtin = thongtin.OrderBy(n => n.IDSANPHAM).ToList();

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
            return RedirectToAction("Khongcoquyen", "QLND");
        }

        // GET: Admin/QLSanPham/PheDuyet
        public ActionResult PheDuyet(int id)
        {
            // Fetch the SANPHAM entity from the database
            var sanPhamEntity = db.SANPHAMs.Where(s => s.IDSANPHAM == id).FirstOrDefault();

            // Create a ViewModelSanPham object and initialize it with the entity data
            var viewModel = new ViewModelSanPham
            {
                SANPHAM = sanPhamEntity
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PheDuyet(ViewModelSanPham model)
        {
            if (ModelState.IsValid)
            {
                // Find the SANPHAM entity using the ID
                var sanPhamEntity = db.SANPHAMs.Find(model.SANPHAM.IDSANPHAM);

                // Update the properties of the entity based on the ViewModel data
                sanPhamEntity.IDPHEDUYET = model.SANPHAM.IDPHEDUYET;

                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("QLSanPham", "QLSanPham");
            }

            return View(model);
        }

    }
}