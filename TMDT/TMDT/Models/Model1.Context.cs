﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TMDTEntities : DbContext
    {
        public TMDTEntities()
            : base("name=TMDTEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ADMIN> ADMINs { get; set; }
        public virtual DbSet<CHUCVU> CHUCVUs { get; set; }
        public virtual DbSet<CTDIENTHOAI> CTDIENTHOAIs { get; set; }
        public virtual DbSet<CTDONHANG> CTDONHANGs { get; set; }
        public virtual DbSet<CTGIAY> CTGIAYs { get; set; }
        public virtual DbSet<CTTHOITRANG> CTTHOITRANGs { get; set; }
        public virtual DbSet<CUAHANG> CUAHANGs { get; set; }
        public virtual DbSet<DANHGIASP> DANHGIASPs { get; set; }
        public virtual DbSet<DONHANG> DONHANGs { get; set; }
        public virtual DbSet<LOAISP> LOAISPs { get; set; }
        public virtual DbSet<MAUSAC> MAUSACs { get; set; }
        public virtual DbSet<NGUOIDUNG> NGUOIDUNGs { get; set; }
        public virtual DbSet<PHEDUYET> PHEDUYETs { get; set; }
        public virtual DbSet<PQNGUOIDUNG> PQNGUOIDUNGs { get; set; }
        public virtual DbSet<PTTHANHTOAN> PTTHANHTOANs { get; set; }
        public virtual DbSet<QUANGCAO> QUANGCAOs { get; set; }
        public virtual DbSet<SANPHAM> SANPHAMs { get; set; }
        public virtual DbSet<SHIPPER> SHIPPERs { get; set; }
        public virtual DbSet<SIZEGIAY> SIZEGIAYs { get; set; }
        public virtual DbSet<SIZETHOITRANG> SIZETHOITRANGs { get; set; }
        public virtual DbSet<THANHPHO> THANHPHOes { get; set; }
        public virtual DbSet<THDIENTHOAI> THDIENTHOAIs { get; set; }
        public virtual DbSet<THGIAY> THGIAYs { get; set; }
        public virtual DbSet<THTHOITRANG> THTHOITRANGs { get; set; }
        public virtual DbSet<TINTUC> TINTUCs { get; set; }
        public virtual DbSet<TRANGTHAIDH> TRANGTHAIDHs { get; set; }
        public virtual DbSet<TRANGTHAIVC> TRANGTHAIVCs { get; set; }
        public virtual DbSet<TRANGTHAIXEMDONHANG> TRANGTHAIXEMDONHANGs { get; set; }
        public virtual DbSet<VOUCHER> VOUCHERs { get; set; }
        public virtual DbSet<VOUCHERSHOP> VOUCHERSHOPs { get; set; }
    }
}
