//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TMDT.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class QUANGCAO
    {
        public int IDQUANGCAO { get; set; }
        public string TIEUDE { get; set; }
        public string HINHANH1 { get; set; }
        public string HINHANH2 { get; set; }
        public string HINHANH3 { get; set; }
        public Nullable<System.DateTime> NGAYBD { get; set; }
        public Nullable<System.DateTime> NGAYKT { get; set; }
        public Nullable<int> IDCUAHANG { get; set; }
        public Nullable<int> IDPHEDUYET { get; set; }
    
        public virtual CUAHANG CUAHANG { get; set; }
        public virtual PHEDUYET PHEDUYET { get; set; }
    }
}
