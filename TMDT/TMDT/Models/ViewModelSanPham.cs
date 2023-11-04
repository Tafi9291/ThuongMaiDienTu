using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TMDT.Models
{
    public class ViewModelSanPham
    {
        public SANPHAM SANPHAM { get; set; }
        public ViewModelSanPham()
        {
            SANPHAM = new SANPHAM();
            SANPHAM.HINHANH1 = "~/Areas/Admin/Content/img/iconanh.png";
            SANPHAM.HINHANH2 = "~/Areas/Admin/Content/img/iconanh.png";
            SANPHAM.HINHANH3 = "~/Areas/Admin/Content/img/iconanh.png";
        }
        [NotMapped]
        public HttpPostedFileBase UploadImage1 { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage2 { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadImage3 { get; set; }
        
        public LOAISP LOAISP { get; set; }
        public CTDIENTHOAI CTDIENTHOAI { get; set; }
        public CTTHOITRANG CTTHOITRANG { get; set; }
        public CTGIAY CTGIAY { get; set; }

        public PHEDUYET PHEDUYET { get; set; }
    }
}