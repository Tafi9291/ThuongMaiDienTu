using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDT.Models
{
    public class ShopViewModel
    {
        public CUAHANG CuaHang { get; set; }
        public IPagedList<SANPHAM> SanPhamPagedList { get; set; }
    }
}