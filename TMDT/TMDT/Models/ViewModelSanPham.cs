using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDT.Models
{
    public class ViewModelSanPham
    {
        public SANPHAM SANPHAM { get; set; }
        public LOAISP LOAISP { get; set; }
        public CTDIENTHOAI CTDIENTHOAI { get; set; }
        public CTTHOITRANG CTTHOITRANG { get; set; }
        public CTGIAY CTGIAY { get; set; }

    }
}