using System;
using System.Collections.Generic;
using System.Text;

namespace My_Profile
{
    // Bảng ThongTinCaNhan
    public class ThongTinCaNhan
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string GhiChu { get; set; }

        // Quan hệ (1 cá nhân có nhiều học vấn, kỹ năng, sở thích, ...)
        public List<HocVan> HocVans { get; set; } = new List<HocVan>();
        public List<KinhNghiemLamViec> KinhNghiems { get; set; } = new List<KinhNghiemLamViec>();
        public List<KyNang> KyNangs { get; set; } = new List<KyNang>();
        public List<SoThich> SoThichs { get; set; } = new List<SoThich>();
    }

    // Bảng HocVan
    public class HocVan
    {
        public int Id { get; set; }
        public int IdCaNhan { get; set; }
        public string Truong { get; set; }
        public string ChuyenNganh { get; set; }
        public string BangCap { get; set; }
        public int? NamBatDau { get; set; }
        public int? NamKetThuc { get; set; }
    }

    // Bảng KinhNghiemLamViec
    public class KinhNghiemLamViec
    {
        public int Id { get; set; }
        public int IdCaNhan { get; set; }
        public string CongTy { get; set; }
        public string ViTri { get; set; }
        public string MoTa { get; set; }
        public int? NamBatDau { get; set; }
        public int? NamKetThuc { get; set; }
    }

    // Bảng KyNang
    public class KyNang
    {
        public int Id { get; set; }
        public int IdCaNhan { get; set; }
        public string TenKyNang { get; set; }
        public string MucDo { get; set; }
    }

    // Bảng SoThich
    public class SoThich
    {
        public int Id { get; set; }
        public int IdCaNhan { get; set; }
        public string TenSoThich { get; set; }
    }
}
