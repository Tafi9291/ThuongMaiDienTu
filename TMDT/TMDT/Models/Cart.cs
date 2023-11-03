using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Models
{
    public class CartItem
    {
        public SANPHAM sanpham { get; set; }
        public int soluong { get; set; }
        public string ErrorMessage { get; set; } // add this property
        public CUAHANG cuahang { get; set; } // Thêm CUAHANG
    }

    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public void Add(SANPHAM sp, int sl = 1)
        {
            var item = items.FirstOrDefault(s => s.sanpham.IDSANPHAM == sp.IDSANPHAM);
            if (item == null)
                items.Add(new CartItem { sanpham = sp, soluong = sl });
            else
                item.soluong += sl;

        }
        public int Tongsoluong()
        {
            return items.Sum(s => s.soluong);
        }
        public void Update_Sl(int id, int sl, ControllerContext controllerContext)
        {
            var item = items.Find(s => s.sanpham.IDSANPHAM == id);
            bool error = false;
            if (item != null)
            {

                if (item.sanpham.SOLUONGTON < sl)
                {

                    item.ErrorMessage = "Số lượng sản phẩm trong kho không đủ";
                    error = true;

                }
                else if (sl <= 0)
                {
                    items.Remove(item);
                }
                else
                {

                    item.soluong = sl;
                }
            }
            if (!error)
            {

                item.ErrorMessage = null;

            }
            //if (error)
            //{
            //    controllerContext.Controller.TempData["Message"] = item.ErrorMessage;
            //}
            //else
            //{
            //    controllerContext.Controller.TempData["Message"] = "Cập nhật số lượng sản phẩm thành công";
            //}
        }
        public Dictionary<int, double> Tongtien()
        {
            var tongTienTheoCuaHang = items.GroupBy(s => s.sanpham.CUAHANG.IDCUAHANG)
                .ToDictionary(
                    group => group.Key, // Key là ID của cửa hàng
                    group => group.Sum(item => (double)item.sanpham.GIAGIAM * item.soluong) // Tổng tiền của cửa hàng
                );
            return tongTienTheoCuaHang;
        }
        public double TongtienCheckout()
        {
            var tongtien = items.Sum(s => s.sanpham.GIAGIAM * s.soluong);
            return (double)tongtien;
        }
        public void Xoasp(int id)
        {
            items.RemoveAll(s => s.sanpham.IDSANPHAM == id);
        }
        public void CLear()
        {
            items.Clear();
        }
        public void Xoagh(int? id)
        {
            items.RemoveAll(s => s.sanpham.CUAHANG.IDCUAHANG == id);
        }
    }
}