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
    
    public partial class ADMIN
    {
        public int IDADMIN { get; set; }
        public string TENAD { get; set; }
        public string SDT { get; set; }
        public string EMAIL { get; set; }
        public string MATKHAU { get; set; }
        public Nullable<int> IDCHUCVU { get; set; }
        public string HINHANH { get; set; }
    
        public virtual CHUCVU CHUCVU { get; set; }
    }
}
