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
    
    public partial class PHEDUYET
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHEDUYET()
        {
            this.QUANGCAOs = new HashSet<QUANGCAO>();
            this.SANPHAMs = new HashSet<SANPHAM>();
        }
        public List<PHEDUYET> ListPD { get; set; }
        public int IDPHEDUYET { get; set; }
        public string TENPHEDUYET { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QUANGCAO> QUANGCAOs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SANPHAM> SANPHAMs { get; set; }
    }
}
