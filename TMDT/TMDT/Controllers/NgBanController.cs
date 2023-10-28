using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
namespace TMDT.Controllers
{
    public class NgBanController : Controller
    {
        TMDTEntities db=new TMDTEntities();
        // GET: NgBan
        public ActionResult KenhNgBan()
        {
            return View();
        }

        // GET: Register
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(NGUOIDUNG shop)
        {
            if (ModelState.IsValid)
            {
                if (db.NGUOIDUNGs.Any(u => u.EMAIL == shop.EMAIL))
                {
                    ViewBag.error = "Email đã tồn tại!";
                    return View();
                }

                if (shop.MATKHAU != shop.NHAPLAIMK)
                {
                    ViewBag.error1 = "Mật khẩu không trùng khớp!";
                    return View();
                }
                shop.IDPQND = 1;
                db.NGUOIDUNGs.Add(shop);
                db.SaveChanges();

                return RedirectToAction("DangNhap", "NgBan");
            }

            return View(shop);
        }
        // GET: NgBan
        public ActionResult DangNhapNgBan()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapNgBan(NGUOIDUNG shop)
        {
            var objUserGet = db.NGUOIDUNGs.Where(n => n.EMAIL.Equals(shop.EMAIL) && n.MATKHAU.Equals(shop.MATKHAU)).FirstOrDefault();
            if (objUserGet == null)
            {
                ViewBag.error = "Email hay Mật khẩu không đúng vui lòng nhập lại!";
                return View();
            }
            if (objUserGet.IDPQND == 1)
            {
                Session["Email"] = objUserGet.EMAIL;
                Session["Ten"] = objUserGet.TENND;
                Session["Hinh"] = objUserGet.HINH;
                Session["Diachi"]=objUserGet.DIACHI;
                Session["SDT"] = objUserGet.SDT;
                Session["ID"] = objUserGet.IDND;
                return RedirectToAction("WelComeBanHang", "NgBan");
            }
            else
            {
                Session["Email"] = objUserGet.EMAIL;
                Session["Ten"] = objUserGet.TENND;
                Session["Hinh"] = objUserGet.HINH;
                Session["ID"] = objUserGet.IDND;
                return RedirectToAction("KenhNgBan", "NgBan");
            }
          

        }
        public ActionResult DangXuat()
        {
            Session["Email"] = null;
            return RedirectToAction("DangNhapNgBan", "NgBan");
        }
        // GET: TaiKhoan
        public ActionResult TaiKhoan()
        {
            // Lấy thông tin khách hàng từ database
            var email = Session["Email"] as string;
            var customer = db.NGUOIDUNGs.FirstOrDefault(c => c.EMAIL == email);
            // Truyền thông tin khách hàng sang
            return View(customer);
        }
        
        [HttpPost]
        [ActionName("CapNhatThongTin")]
        public ActionResult CapNhatThongTin(NGUOIDUNG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.NGUOIDUNGs.Find(model.IDND);
                //var edituser = db.KHACHHANGs.Where(x => x.KHACHHANGID == kh.KHACHHANGID).FirstOrDefault();
                if (model.UploadImage1 != null)
                {
                    string filename1 = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                    string extension1 = Path.GetExtension(model.UploadImage1.FileName);
                    filename1 = filename1 + extension1;
                    model.HINH = "~/Content/Hinh/" + filename1;
                    model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
                    // gan cac du lieu vao cai lay len

                }

                objUser.IDND = model.IDND;
               
                objUser.TENND = model.TENND;
               
                
              
                objUser.HINH = model.HINH;
               
                Session["Ten"] = model.TENND;
                Session["Hinh"] = model.HINH;
                db.SaveChanges();
                return RedirectToAction("TaiKhoan", "NgBan");
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("SDT")]
        public ActionResult SDT(NGUOIDUNG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.NGUOIDUNGs.Find(model.IDND);
               

                objUser.IDND = model.IDND;

                objUser.SDT = model.SDT;



               
                db.SaveChanges();
                return RedirectToAction("TaiKhoan", "NgBan");
            }

            return View(model);
        }

        

        [HttpPost]
        [ActionName("DiaChi")]
        public ActionResult DiaChi(NGUOIDUNG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.NGUOIDUNGs.Find(model.IDND);
               

                objUser.IDND = model.IDND;

               



                objUser.DIACHI = model.DIACHI;

                
                db.SaveChanges();
                return RedirectToAction("TaiKhoan", "NgBan");
            }

            return View(model);
        }

        // GET: Welcome
        public ActionResult WelComeBanHang()
        {
            return View();
        }

        // GET: DangKyNgBan
        public ActionResult DangKyBanHang()
        {
            // Lấy thông tin khách hàng từ database
            CUAHANG shop = new CUAHANG();
            shop.TENCH = Session["Ten"].ToString(); 
            shop.DIACHI = Session["Diachi"].ToString() ;
            
            return View(shop);

        }
        [HttpPost]
        public ActionResult DangKyBanHang(CUAHANG shop)
        {
            if (ModelState.IsValid)
            {
                // Lấy ID của người dùng từ Session
                int userID = (int)Session["ID"];
                // Tìm người dùng theo ID của người dùng, hoặc theo điều kiện phù hợp
                NGUOIDUNG user = db.NGUOIDUNGs.FirstOrDefault(u => u.IDND == userID);

                if (user != null)
                {
                    // Kiểm tra xem IDPQND hiện tại của người dùng có phải là 1 không
                    if (user.IDPQND == 1)
                    {
                        // Cập nhật IDPQND thành 2
                        user.IDPQND = 2;
                    }
                    shop.IDND = userID;
                    // Thêm cửa hàng vào cơ sở dữ liệu
                    db.CUAHANGs.Add(shop);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    return RedirectToAction("KenhNgBan", "NgBan");
                }
            }

            return View(shop);
        }

        // GET: CuaHang
        public ActionResult CuaHang()
        {
            return View();
        }

    }
}