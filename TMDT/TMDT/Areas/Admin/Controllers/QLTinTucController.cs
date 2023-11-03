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
    public class QLTinTucController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        // GET: Admin/QLTinTuc 
        public ActionResult TinTuc(int? size, int? page, string currentFilter, string searchString)
        {
            // Kiểm tra quyền truy cập theo CHUCVUID 

            var email = Session["Email"] as string;
            var admin = db.ADMINs.FirstOrDefault(c => c.EMAIL == email);
            if (admin != null && (admin.IDCHUCVU == 1 || admin.IDCHUCVU == 3))
            {
                var thongtin = new List<TINTUC>();
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
                    thongtin = db.TINTUCs.Where(n => n.TIEUDE.Contains(searchString)).ToList();

                }
                else
                {
                    //lấy ds tên nv theo bảng NV 
                    thongtin = db.TINTUCs.ToList();
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
                thongtin = thongtin.OrderBy(n => n.IDTINTUC).ToList();

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
        public ActionResult TaoTinTuc()
        {

            TINTUC pro = new TINTUC();
            return View(pro);
        }

        // POST: Admin/Dienthoai/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult TaoTinTuc(TINTUC model)
        {
            if (ModelState.IsValid)
            {
                // Lưu ảnh vào thư mục ~/Content/Images/ 
                if (model.UploadImage1 != null)
                {
                    string filename1 = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                    string extension1 = Path.GetExtension(model.UploadImage1.FileName);
                    filename1 = filename1 + extension1;
                    model.HINHANH = "~/Content/Hinh/" + filename1;
                    model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
                    // gan cac du lieu vao cai lay len 

                }
                db.TINTUCs.Add(model);
                db.SaveChanges();
                return RedirectToAction("TinTuc");
            }

            return View(model);
        }
        public ActionResult CapNhatTinTuc(int id)
        {
            return View(db.TINTUCs.Where(s => s.IDTINTUC == id).FirstOrDefault());
        }

        // POST: Admin/Hang/Edit/5 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatTinTuc(TINTUC model)
        {
            if (ModelState.IsValid)
            {
                var tt = db.TINTUCs.Find(model.IDTINTUC);

                if (model.UploadImage1 != null)
                {
                    string filename1 = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                    string extension1 = Path.GetExtension(model.UploadImage1.FileName);
                    filename1 = filename1 + extension1;
                    model.HINHANH = "~/Content/Hinh/" + filename1;
                    model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
                    // gan cac du lieu vao cai lay len 

                }

                tt.MOTA = model.MOTA;
                tt.TIEUDE = model.TIEUDE;
                tt.HINHANH = model.HINHANH;


                db.SaveChanges();
                return RedirectToAction("TinTuc", "QLTinTuc");
            }

            return View(model);
        }
    }
}