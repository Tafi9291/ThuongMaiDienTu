using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMDT.Models
{
    public class DashBoard
    {
        public int TotalUsers { get; set; }
        public int TotalAdmin { get; set; }
        public int TotalNewOrders { get; set; }
        public int TotalWaitingOrders { get; set; }
        public int TotalGotOrders { get; set; }
        public int TotalOnWayOrders { get; set; }
        public int TotalCompleteOrders { get; set; }
        public int TotalCancelOrders { get; set; }
        public int ProductOutOfSold { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUnpaid { get; set; }
        public int TotalPaid{ get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0 đ}", ApplyFormatInEditMode = false)]
        public Nullable<decimal> TotalRevenue { get; set; }
    }
}