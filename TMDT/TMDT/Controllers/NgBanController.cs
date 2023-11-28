using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using TMDT.Models;
using System.Web.UI.DataVisualization.Charting;
using Chart = System.Web.UI.DataVisualization.Charting.Chart;

namespace TMDT.Controllers
{
    public class NgBanController : Controller
    {
        TMDTEntities db=new TMDTEntities();
        // GET: NgBan
        public ActionResult KenhNgBan()
        {
            var model = new DashBoard();

            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhapNgBan", "NgBan");
            }
            // Lấy thông tin của người dùng trong cơ sở dữ liệu
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null) // Kiểm tra xem người dùng có tồn tại hay không
            {
                // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
                var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
                {
                    model.TotalNewOrders = db.DONHANGs.Where(x => x.IDTRANGTHAIDH == 1 && x.IDCUAHANG == cuahang.IDCUAHANG).Count();
                    model.TotalWaitingOrders = db.DONHANGs.Where(x => x.IDTRANGTHAIDH == 2 && x.IDCUAHANG == cuahang.IDCUAHANG).Count();
                    model.TotalGotOrders = db.DONHANGs.Where(x => x.IDTRANGTHAIDH == 3 && x.IDCUAHANG == cuahang.IDCUAHANG).Count();
                    model.TotalOnWayOrders = db.DONHANGs.Where(x => x.IDTRANGTHAIDH == 4 && x.IDCUAHANG == cuahang.IDCUAHANG).Count();
                    model.TotalCompleteOrders = db.DONHANGs.Where(x => x.IDTRANGTHAIDH == 6 && x.IDCUAHANG == cuahang.IDCUAHANG).Count();
                    model.TotalCancelOrders = db.DONHANGs.Where(x => x.IDTRANGTHAIDH == 8 && x.IDCUAHANG == cuahang.IDCUAHANG).Count();

                    model.ProductOutOfSold = db.SANPHAMs.Where(x => x.SOLUONGTON <= 0 && x.IDCUAHANG == cuahang.IDCUAHANG).Count();

                    return View(model);
                }
            }
            return RedirectToAction("Error");
        }

        public ActionResult Chart()
        {
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhapNgBan", "NgBan");
            }
            // Lấy thông tin của người dùng trong cơ sở dữ liệu
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null) // Kiểm tra xem người dùng có tồn tại hay không
            {
                // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
                var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
                {
                    var data = db.DONHANGs.Where(x => x.IDTRANGTHAIDH == 6 && x.IDCUAHANG == cuahang.IDCUAHANG).ToList();
                    var chart = new Chart();
                    var area = new ChartArea();
                    area.AxisX.Minimum = 1;
                    area.AxisX.Maximum = 12;
                    area.AxisY.Minimum = 0;
                    area.AxisY.Maximum = (double)data.Sum(x => x.THANHTIEN);
                    area.AxisX.Interval = 1;
                    area.AxisY.Interval = (double)(data.Sum(x => x.THANHTIEN) / 10);
                    area.AxisX.MajorGrid.Enabled = false;
                    area.AxisY.MajorGrid.Enabled = true;
                    chart.Width = 800;
                    chart.Height = 500;
                    chart.ChartAreas.Add(area);
                    var series = new Series();
                    var year = DateTime.Now.Year;
                    var ordersByMonth = data.GroupBy(x => x.NGAYTAO.Value.Month);
                    foreach (var item in ordersByMonth)
                    {
                        var month = item.Key;
                        var totalRevenue = item.Sum(x => x.THANHTIEN);
                        series.Points.AddXY(month, totalRevenue);
                    }
                    Color[] colors = new Color[] { Color.LightGreen, Color.MistyRose, Color.Blue };
                    for (int i = 0; i < series.Points.Count; i++)
                    {
                        series.Points[i].Color = colors[i];
                    }
                    // Đặt tiêu đề của biểu đồ và cỡ chữ
                    var chartTitle = new Title();
                    chartTitle.Text = $"Doanh thu theo tháng năm {year}";
                    chartTitle.Font = new Font("Arial", 20, FontStyle.Bold); // Đặt cỡ chữ ở đây
                    chart.Titles.Add(chartTitle);
                    series.Label = "#PERCENT{P0}";
                    series.Font = new Font("Arial", 10, FontStyle.Bold);
                    series.ChartType = SeriesChartType.Column;
                    series["PieLabelStyle"] = "Outside";
                    chart.Series.Add(series);
                    area.AxisY.LabelStyle.Format = "{N0} đ"; // Add thousand separator
                    var returnStream = new MemoryStream();
                    chart.ImageType = ChartImageType.Png;
                    chart.SaveImage(returnStream);
                    returnStream.Position = 0;
                    return new FileStreamResult(returnStream, "image/png");
                }
            }
            return RedirectToAction("Error");
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

                return RedirectToAction("DangNhapNgBan  ", "NgBan");
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
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhapNgBan", "NgBan");
            }

            // Lấy thông tin của người dùng trong cơ sở dữ liệu
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);

            if (nguoidung != null) // Kiểm tra xem người dùng có tồn tại hay không
            {
                // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
                var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
                {
                    // Lấy thông tin chi tiết của cửa hàng
                    var cuahangChiTiet = db.CUAHANGs.SingleOrDefault(ch => ch.IDCUAHANG == cuahang.IDCUAHANG);

                    if (cuahangChiTiet != null)
                    {
                        return View(cuahangChiTiet);
                    }
                }
            }
            return View();
        }
        [HttpPost]
        [ActionName("CapNhatCuaHang")]
        public ActionResult CapNhatCuaHang(CUAHANG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.CUAHANGs.Find(model.IDCUAHANG);
               
                if (model.UploadImage1 != null)
                {
                    string filename1 = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                    string extension1 = Path.GetExtension(model.UploadImage1.FileName);
                    filename1 = filename1 + extension1;
                    model.ANHDAIDIEN = "~/Content/Hinh/" + filename1;
                    model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
                    // gan cac du lieu vao cai lay len

                }
                objUser.TENCH = model.TENCH;
                objUser.ANHDAIDIEN = model.ANHDAIDIEN;
                db.SaveChanges();
                return RedirectToAction("CuaHang", "NgBan");
            }

            return View(model);
        }
        [HttpPost]
        [ActionName("CapNhatAnhNen")]
        public ActionResult CapNhatAnhNen(CUAHANG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.CUAHANGs.Find(model.IDCUAHANG);

                if (model.UploadImage2 != null)
                {
                    string filename2 = Path.GetFileNameWithoutExtension(model.UploadImage2.FileName);
                    string extension2 = Path.GetExtension(model.UploadImage2.FileName);
                    filename2 = filename2 + extension2;
                    model.HINHANH = "~/Content/Hinh/" + filename2;
                    model.UploadImage2.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename2));
                    // gan cac du lieu vao cai lay len

                }
                
                objUser.HINHANH = model.HINHANH;
                db.SaveChanges();
                return RedirectToAction("CuaHang", "NgBan");
            }

            return View(model);
        }
        [HttpPost]
        [ActionName("CapNhatDiaChi")]
        public ActionResult CapNhatDiaChi(CUAHANG model)
        {
            if (ModelState.IsValid)
            {
                var objUser = db.CUAHANGs.Find(model.IDCUAHANG);
                objUser.DIACHI = model.DIACHI;
                db.SaveChanges();
                return RedirectToAction("CuaHang", "NgBan");
            }

            return View(model);
        }

        public ActionResult DoanhThu()
        {
            var model = new DashBoard();

            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhapNgBan", "NgBan");
            }
            // Lấy thông tin của người dùng trong cơ sở dữ liệu
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null) // Kiểm tra xem người dùng có tồn tại hay không
            {
                // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
                var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
                {
                    model.TotalUnpaid = (int)db.DONHANGs.Where(x => x.IDTRANGTHAIDH == 1 && x.IDCUAHANG == cuahang.IDCUAHANG).Sum(x => x.THANHTIEN);
                    model.TotalPaid = (int)db.DONHANGs.Where(x => x.IDTRANGTHAIDH == 6 && x.IDCUAHANG == cuahang.IDCUAHANG).Sum(x => x.THANHTIEN);

                    return View(model);
                }
            }
            return RedirectToAction("Error");
        }

    }
}