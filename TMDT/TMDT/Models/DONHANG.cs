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
    
    public partial class DONHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONHANG()
        {
            this.CTDONHANGs = new HashSet<CTDONHANG>();
        }
    
        public int IDDONHANG { get; set; }
        public Nullable<System.DateTime> NGAYTAO { get; set; }
        public string NGUOINHAN { get; set; }
        public string DIACHI { get; set; }
        public string SDT { get; set; }
        public Nullable<int> IDND { get; set; }
        public Nullable<int> IDSHIPPER { get; set; }
        public Nullable<int> IDPTTHANHTOAN { get; set; }
        public Nullable<int> IDVOUCHERSHOP { get; set; }
        public Nullable<int> IDVOUCHER { get; set; }
        public Nullable<int> IDTRANGTHAIDH { get; set; }
        public Nullable<int> IDTRANGTHAIXDH { get; set; }
        public Nullable<int> THANHTIEN { get; set; }
        public Nullable<int> IDCUAHANG { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDONHANG> CTDONHANGs { get; set; }
        public virtual CUAHANG CUAHANG { get; set; }
        public virtual NGUOIDUNG NGUOIDUNG { get; set; }
        public virtual PTTHANHTOAN PTTHANHTOAN { get; set; }
        public virtual SHIPPER SHIPPER { get; set; }
        public virtual TRANGTHAIDH TRANGTHAIDH { get; set; }
        public virtual TRANGTHAIXEMDONHANG TRANGTHAIXEMDONHANG { get; set; }
        public virtual VOUCHERSHOP VOUCHERSHOP { get; set; }
        public virtual VOUCHER VOUCHER { get; set; }
    }
}
