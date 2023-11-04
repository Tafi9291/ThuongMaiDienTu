using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class SanPhamController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        // GET: SanPham
        public ActionResult SanPham(int? size, int? page, string currentFilter, string searchString)
        {
            SANPHAM sp = new SANPHAM();
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "Register");
            }
            // Lấy thông tin của người dùng trong cơ sở dữ liệu
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);
            if (nguoidung != null) // Kiểm tra xem người dùng có tồn tại hay không
            {
                // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
                var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
                {
                    sp.IDCUAHANG = cuahang.IDCUAHANG;
                    // Lấy danh sách sản phẩm theo IDCUAHANG
                    var products = db.SANPHAMs
                        .Where(p => p.IDCUAHANG == sp.IDCUAHANG)
                        .OrderByDescending(p => p.TENSP);

                    // Thực hiện tìm kiếm nếu có chuỗi tìm kiếm
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        products = (IOrderedQueryable<SANPHAM>)products.Where(p => p.TENSP.Contains(searchString));
                    }

                    // Lưu trạng thái tìm kiếm hiện tại
                    currentFilter = searchString;

                    var pageNumber = page ?? 1;
                    int pageSize = size ?? 10;
                    var pagedVouchers = products
                        .ToPagedList(pageNumber, pageSize);

                    return View(pagedVouchers);

                }
            }
            ////const int pageSize = 10;
            ////var pageNumber = (page ?? 1);
            ////var products = db.SANPHAMs.OrderBy(p => p.TENSP).ToList().ToPagedList(pageNumber, pageSize);
            //return RedirectToAction("");
            return RedirectToAction("ErrorPage"); // Xử lý khi không tìm thấy cửa hàng hoặc người dùng
        }

        public ActionResult ChonNganhHang()
        {
            LOAISP loaisp = new LOAISP();
            loaisp.ListCate = db.LOAISPs.ToList<LOAISP>();
            return PartialView(loaisp);
        }

        public ActionResult THDienThoai()
        {
            THDIENTHOAI tHDIENTHOAI = new THDIENTHOAI();
            tHDIENTHOAI.ListBrand = db.THDIENTHOAIs.ToList<THDIENTHOAI>();
            return PartialView(tHDIENTHOAI);
        }

        public ActionResult MauSac()
        {
            MAUSAC mausac = new MAUSAC();
            mausac.ListColor = db.MAUSACs.ToList<MAUSAC>();
            return PartialView(mausac);
        }

        public ActionResult THThoiTrang()
        {
            THTHOITRANG tHTHOITRANG = new THTHOITRANG();
            tHTHOITRANG.ListBrands = db.THTHOITRANGs.ToList<THTHOITRANG>();
            return PartialView(tHTHOITRANG);
        }

        public ActionResult SizeThoiTrang()
        {
            SIZETHOITRANG sIZETHOITRANG = new SIZETHOITRANG();
            sIZETHOITRANG.ListSize = db.SIZETHOITRANGs.ToList<SIZETHOITRANG>();
            return PartialView(sIZETHOITRANG);
        }

        public ActionResult THGiay()
        {
            THGIAY tHGIAY = new THGIAY();
            tHGIAY.ListBrands = db.THGIAYs.ToList<THGIAY>();
            return PartialView(tHGIAY);
        }

        public ActionResult SizeGiay()
        {
            SIZEGIAY sIZEGIAY = new SIZEGIAY();
            sIZEGIAY.ListSize = db.SIZEGIAYs.ToList<SIZEGIAY>();
            return PartialView(sIZEGIAY);
        }

        // GET: SanPham/Create
        public ActionResult TaoSanPham() 
        {
            ViewModelSanPham sp = new ViewModelSanPham();
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoSanPham(ViewModelSanPham model)
        {
            var email = Session["Email"] as string;
            if (email == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Dangnhap", "DNhap");
            }

            // Lấy thông tin của người dùng trong cơ sở dữ liệu
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == email);

            if (nguoidung != null) // Kiểm tra xem người dùng có tồn tại hay không
            {
                // Lấy thông tin của cửa hàng nếu tồn tại, sau đó lưu ID của cửa hàng vào sản phẩm
                var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                if (cuahang != null) // Kiểm tra xem cửa hàng có tồn tại hay không
                {
                    model.SANPHAM.IDCUAHANG = cuahang.IDCUAHANG;

                }
            }
            //// Lưu ảnh vào thư mục ~/Content/Data/Hinh
            if (model.UploadImage1 != null && model.UploadImage2 != null && model.UploadImage3 != null)
            {
                string filename1 = Path.GetFileNameWithoutExtension(model.UploadImage1.FileName);
                string extension1 = Path.GetExtension(model.UploadImage1.FileName);
                filename1 = filename1 + extension1;
                string filename2 = Path.GetFileNameWithoutExtension(model.UploadImage2.FileName);
                string extension2 = Path.GetExtension(model.UploadImage2.FileName);
                filename2 = filename2 + extension2;
                string filename3 = Path.GetFileNameWithoutExtension(model.UploadImage3.FileName);
                string extension3 = Path.GetExtension(model.UploadImage3.FileName);
                filename3 = filename3 + extension3;

                model.SANPHAM.HINHANH1 = "~/Content/Hinh/" + filename1;
                model.SANPHAM.HINHANH2 = "~/Content/Hinh/" + filename2;
                model.SANPHAM.HINHANH3 = "~/Content/Hinh/" + filename3;

                model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
                model.UploadImage2.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename2));
                model.UploadImage3.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename3));
            }
                // Kiểm tra giá bán, số lượng tồn và phần trăm giảm
                if (model.SANPHAM.GIABAN >= 0 && model.SANPHAM.SOLUONGTON >= 0 && model.SANPHAM.PHANTRAMGIAM >= 0 && model.SANPHAM.PHANTRAMGIAM <= 100)
                {

                    model.SANPHAM.GIAGIAM = model.SANPHAM.GIABAN - (model.SANPHAM.GIABAN * model.SANPHAM.PHANTRAMGIAM / 100);

                    // Đặt ngày tạo hiện tại
                    model.SANPHAM.NGAYTAO = DateTime.Now;
                    model.SANPHAM.IDPHEDUYET = 1;
                    CTGIAY ct = new CTGIAY();
                    model.CTGIAY = ct;

                    string selectedCategoryId = Request.Form["IDLOAISP"];
                    model.SANPHAM.IDLOAISP = Convert.ToInt32(selectedCategoryId);
                

                    string selectedColor = Request.Form["IDMAUSAC"];
                    model.SANPHAM.IDMAUSAC = Convert.ToInt32(selectedColor);

                    string selecteDBrand = Request.Form["IDTHDIENTHOAI"];

                    string selectTTBrand = Request.Form["IDTHTHOITRANG"];

                    string selectGBrand = Request.Form["IDTHGIAY"];

                    string selectSizeTT = Request.Form["IDSIZETT"];
                
                    string selectSizeG = Request.Form["IDSIZEGIAY"];


                // Thêm vào cơ sở dữ liệu dựa trên LoaiSP
                switch (selectedCategoryId)
                    {
                        case "1":
                            model.CTDIENTHOAI.IDLOAISP = Convert.ToInt32(selectedCategoryId);
                            model.CTDIENTHOAI.IDTHDIENTHOAI = Convert.ToInt32(selecteDBrand);
                            db.CTDIENTHOAIs.Add(model.CTDIENTHOAI);
                            int idSANPHAM = model.SANPHAM.IDSANPHAM;
                            model.CTDIENTHOAI.IDSANPHAM = idSANPHAM;
                            break;
                        case "2":
                            model.CTTHOITRANG.IDLOAISP = Convert.ToInt32(selectedCategoryId);
                            model.CTTHOITRANG.IDTHTHOITRANG = Convert.ToInt32(selectTTBrand);
                            model.CTTHOITRANG.IDSIZETT = Convert.ToInt32(selectSizeTT);
                            db.CTTHOITRANGs.Add(model.CTTHOITRANG);
                            int idSANPHAM2 = model.SANPHAM.IDSANPHAM;
                            model.CTTHOITRANG.IDSANPHAM = idSANPHAM2;
                            break;
                        case "3":
                            ct.IDLOAISP = Convert.ToInt32(selectedCategoryId);
                            ct.IDTHGIAY = Convert.ToInt32(selectGBrand);
                            ct.IDSIZEGIAY = Convert.ToInt32(selectSizeG);
                            db.CTGIAYs.Add(model.CTGIAY);
                            int idSANPHAM3 = model.SANPHAM.IDSANPHAM;
                            model.CTGIAY.IDSANPHAM = idSANPHAM3;
                            break;
                        default:
                            // Xử lý mặc định hoặc lỗi nếu có
                            break;
                    }
                    db.SANPHAMs.Add(model.SANPHAM);
                    db.SaveChanges();
                    return RedirectToAction("SanPham");
                }
                else
                {
                    if (model.SANPHAM.GIABAN < 0)
                    {
                        ModelState.AddModelError("GIABAN", "Giá bán phải lớn hơn hoặc bằng 0.");
                    }
                    if (model.SANPHAM.SOLUONGTON < 0)
                    {
                        ModelState.AddModelError("SOLUONGTON", "Số lượng tồn phải lớn hơn hoặc bằng 0.");
                    }
                    if (model.SANPHAM.PHANTRAMGIAM < 0 || model.SANPHAM.PHANTRAMGIAM > 100)
                    {
                        ModelState.AddModelError("PHANTRAMGIAM", "Phần trăm giảm phải trong khoảng từ 0 đến 100.");
                    }
                }

            return View(model);
        }

        public ActionResult CapNhat(int id)
        {
            // Fetch the SANPHAM entity from the database 
            var sanPhamEntity = db.SANPHAMs.Where(s => s.IDSANPHAM == id).FirstOrDefault();
            var ctDienThoai = db.CTDIENTHOAIs.Where(s => s.IDSANPHAM == id).FirstOrDefault();
            var ctThoiTrang = db.CTTHOITRANGs.Where(s => s.IDSANPHAM == id).FirstOrDefault();
            var ctGiay = db.CTGIAYs.Where(s => s.IDSANPHAM == id).FirstOrDefault();

            // Create a ViewModelSanPham object and initialize it with the entity data 
            var viewModel = new ViewModelSanPham
            {
                SANPHAM = sanPhamEntity,
                CTDIENTHOAI = ctDienThoai,
                CTTHOITRANG = ctThoiTrang,
                CTGIAY = ctGiay,
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CapNhat(ViewModelSanPham model)
        {

            try
            {
                // Find the SANPHAM entity using the ID 
                var sanPhamEntity = db.SANPHAMs.Find(model.SANPHAM.IDSANPHAM);
                var ctDienThoai = db.CTDIENTHOAIs.Find(model.CTDIENTHOAI.IDSANPHAM);
                var ctThoiTrang = db.CTTHOITRANGs.Find(model.CTTHOITRANG.IDSANPHAM);
                var ctGiay = db.CTGIAYs.Find(model.CTGIAY.IDSANPHAM);

                sanPhamEntity.HINHANH1 = model.SANPHAM.HINHANH1;
                sanPhamEntity.HINHANH2 = model.SANPHAM.HINHANH2;
                sanPhamEntity.HINHANH3 = model.SANPHAM.HINHANH3;
                sanPhamEntity.TENSP = model.SANPHAM.TENSP;
                sanPhamEntity.IDMAUSAC = model.SANPHAM.IDMAUSAC;
                sanPhamEntity.MOTASANPHAM = model.SANPHAM.MOTASANPHAM;

                string selectedCategoryId = Request.Form["IDLOAISP"];
                sanPhamEntity.IDLOAISP = Convert.ToInt32(selectedCategoryId);

                switch (selectedCategoryId)
                {
                    case "1":
                        ctDienThoai.IDTHDIENTHOAI = model.CTDIENTHOAI.IDTHDIENTHOAI;
                        ctDienThoai.MANHINH = model.CTDIENTHOAI.MANHINH;
                        ctDienThoai.DOPHANGIAI = model.CTDIENTHOAI.DOPHANGIAI;
                        ctDienThoai.CAMERA = model.CTDIENTHOAI.CAMERA;
                        ctDienThoai.HEDH = model.CTDIENTHOAI.HEDH;
                        ctDienThoai.CHIPXULY = model.CTDIENTHOAI.CHIPXULY;
                        ctDienThoai.ROM = model.CTDIENTHOAI.ROM;
                        ctDienThoai.RAM = model.CTDIENTHOAI.RAM;
                        ctDienThoai.MANGDIDONG = model.CTDIENTHOAI.MANGDIDONG;
                        ctDienThoai.SOKHESIM = model.CTDIENTHOAI.SOKHESIM;
                        ctDienThoai.PIN = model.CTDIENTHOAI.PIN;
                        break;
                    case "2":
                        ctThoiTrang.IDTHTHOITRANG = model.CTTHOITRANG.IDTHTHOITRANG;
                        ctThoiTrang.CHATLIEU = model.CTTHOITRANG.CHATLIEU;
                        ctThoiTrang.IDSIZETT = model.CTTHOITRANG.IDSIZETT;
                        break;
                    case "3":
                        ctGiay.IDTHGIAY = model.CTGIAY.IDTHGIAY;
                        ctGiay.IDSIZEGIAY = model.CTGIAY.IDSIZEGIAY;
                        break;
                    default:
                        break;
                }


                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("SanPham");
            }
            catch
            {
                return View(model);

            }
        }
    }
}