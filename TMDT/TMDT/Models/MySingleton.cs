using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;

namespace TMDT.Models
{
    public sealed class MySingleton
    {
        TMDTEntities db = new TMDTEntities();
        public static MySingleton Instance { get; } = new MySingleton();
        public List<SANPHAM> List { get; set; } = new List<SANPHAM>();
        private MySingleton() { }
        private string userEmail;

        // only one time
        public void Init(string userEmail)
        {
            this.userEmail = userEmail;

        }
        // Additional methods for data retrieval or manipulation can be added here
        public List<SANPHAM> GetSanPhamList()
        {
            // Add your logic to retrieve the SANPHAM list based on userEmail or any other criteria
            // For example:
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(kh => kh.EMAIL == userEmail);
            if (nguoidung != null)
            {
                var cuahang = db.CUAHANGs.SingleOrDefault(ch => ch.IDND == nguoidung.IDND);

                if (cuahang != null)
                {
                    List = db.SANPHAMs
                        .Where(p => p.IDCUAHANG == cuahang.IDCUAHANG)
                        .OrderByDescending(p => p.TENSP)
                        .ToList();
                }
            }

            return List;
        }
    }
}