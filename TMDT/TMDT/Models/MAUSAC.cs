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
    
    public partial class MAUSAC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MAUSAC()
        {
            this.CTDIENTHOAIs = new HashSet<CTDIENTHOAI>();
            this.CTGIAYs = new HashSet<CTGIAY>();
            this.CTTHOITRANGs = new HashSet<CTTHOITRANG>();
        }
        public List<MAUSAC> ListColor { get; set; }
        public int IDMAUSAC { get; set; }
        public string MAUSAC1 { get; set; }
        public Nullable<int> IDLOAISP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDIENTHOAI> CTDIENTHOAIs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTGIAY> CTGIAYs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTTHOITRANG> CTTHOITRANGs { get; set; }
        public virtual LOAISP LOAISP { get; set; }
    }
}
