using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class SanPhamController : Controller
    {
        TMDTEntities db = new TMDTEntities();
        // GET: SanPham
        public ActionResult SanPham(int? page)
        {
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            var products = db.SANPHAMs.OrderBy(p => p.TENSP).ToList().ToPagedList(pageNumber, pageSize);
            return View(products);
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

        // GET: SanPham/Create
        public ActionResult TaoSanPham() 
        {
            SANPHAM sp = new SANPHAM();
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoSanPham(SANPHAM model)
        {

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

                model.HINHANH1 = "~/Content/Hinh/" + filename1;
                model.HINHANH2 = "~/Content/Hinh/" + filename2;
                model.HINHANH3 = "~/Content/Hinh/" + filename3;

                model.UploadImage1.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename1));
                model.UploadImage2.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename2));
                model.UploadImage3.SaveAs(Path.Combine(Server.MapPath("~/Content/Hinh/"), filename3));
            }
                // Kiểm tra giá bán, số lượng tồn và phần trăm giảm
                if (model.GIABAN >= 0 && model.SOLUONGTON >= 0 && model.PHANTRAMGIAM >= 0 && model.PHANTRAMGIAM <= 100)
                {

                    model.GIAGIAM = model.GIABAN - (model.GIABAN * model.PHANTRAMGIAM / 100);

                    // Đặt ngày tạo hiện tại
                    model.NGAYTAO = DateTime.Now;
                    model.IDPHEDUYET = 1;

                    string selectedCategoryId = Request.Form["IDLOAISP"];
                    model.IDLOAISP = Convert.ToInt32(selectedCategoryId);
                

                    string selectedColor = Request.Form["IDMAUSAC"];

                    string selecteDBrand = Request.Form["IDTHDIENTHOAI"];
                
                    // Thêm vào cơ sở dữ liệu dựa trên LoaiSP
                    switch (selectedCategoryId)
                    {
                        case "1":
                            model.CTDIENTHOAI.IDMAUSAC = Convert.ToInt32(selectedColor); // Gán giá trị màu sắc
                            model.CTDIENTHOAI.IDLOAISP = Convert.ToInt32(selectedCategoryId);
                            model.CTDIENTHOAI.IDTHDIENTHOAI = Convert.ToInt32(selecteDBrand);
                            model.IDCTDIENTHOAI = 1;
                            db.CTDIENTHOAIs.Add(model.CTDIENTHOAI);
                            break;
                        case "2":
                            db.CTTHOITRANGs.Add(model.CTTHOITRANG);
                            break;
                        case "3":
                            db.CTGIAYs.Add(model.CTGIAY);
                            break;
                        default:
                            // Xử lý mặc định hoặc lỗi nếu có
                            break;
                    }
                    db.SANPHAMs.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("SanPham");
                }
                else
                {
                    if (model.GIABAN < 0)
                    {
                        ModelState.AddModelError("GIABAN", "Giá bán phải lớn hơn hoặc bằng 0.");
                    }
                    if (model.SOLUONGTON < 0)
                    {
                        ModelState.AddModelError("SOLUONGTON", "Số lượng tồn phải lớn hơn hoặc bằng 0.");
                    }
                    if (model.PHANTRAMGIAM < 0 || model.PHANTRAMGIAM > 100)
                    {
                        ModelState.AddModelError("PHANTRAMGIAM", "Phần trăm giảm phải trong khoảng từ 0 đến 100.");
                    }
                }

            return View(model);
        }
    }
}