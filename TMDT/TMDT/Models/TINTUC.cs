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
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class TINTUC
    {
        public TINTUC()
        {
            HINHANH = "~/Areas/Admin/Content/img/iconanh.png";
        }
        [NotMapped]
        public HttpPostedFileBase UploadImage1 { get; set; }
        public int IDTINTUC { get; set; }
        public string TIEUDE { get; set; }
        public string MOTA { get; set; }
        public string HINHANH { get; set; }
    }
}
