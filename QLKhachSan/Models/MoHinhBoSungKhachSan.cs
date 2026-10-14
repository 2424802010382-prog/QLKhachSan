using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKhachSan.Models
{
    // =====================================================
    // BẢNG DỊCH VỤ
    // =====================================================

    [Table("DichVu")]
    public class DichVu
    {
        [Key]
        public int MaDichVu { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDichVu { get; set; }

        [StringLength(500)]
        public string MoTa { get; set; }

        public decimal Gia { get; set; }

        [StringLength(50)]
        public string DonVi { get; set; }

        public bool HoatDong { get; set; }

        public virtual ICollection<DatPhongDichVu>
            DatPhongDichVus
        { get; set; }

        public DichVu()
        {
            DatPhongDichVus =
                new HashSet<DatPhongDichVu>();
        }
    }


    // =====================================================
    // BẢNG KHUYẾN MÃI
    // =====================================================

    [Table("KhuyenMai")]
    public class KhuyenMai
    {
        [Key]
        public int MaKhuyenMai { get; set; }

        [Required]
        [StringLength(30)]
        public string MaCode { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhuyenMai { get; set; }

        [StringLength(500)]
        public string MoTa { get; set; }

        // PhanTram hoặc SoTien
        [Required]
        [StringLength(20)]
        public string LoaiGiamGia { get; set; }

        public decimal GiaTriGiam { get; set; }

        public decimal DonToiThieu { get; set; }

        public decimal? GiamToiDa { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public int? SoLuotSuDungToiDa { get; set; }

        public int SoLuotDaSuDung { get; set; }

        public bool HoatDong { get; set; }
    }


    // =====================================================
    // BẢNG ĐẶT PHÒNG - DỊCH VỤ
    // =====================================================

    [Table("DatPhongDichVu")]
    public class DatPhongDichVu
    {
        [Key]
        public int MaDatPhongDichVu { get; set; }

        // Khóa ngoại tới bảng Bookings
        public int BookingId { get; set; }

        // Khóa ngoại tới bảng DichVu
        public int MaDichVu { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        public decimal ThanhTien { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking DatPhong { get; set; }

        [ForeignKey("MaDichVu")]
        public virtual DichVu DichVu { get; set; }
    }


    // =====================================================
    // BẢNG THANH TOÁN
    // =====================================================

    [Table("ThanhToan")]
    public class ThanhToan
    {
        [Key]
        public int MaThanhToan { get; set; }

        // Khóa ngoại tới Booking
        public int BookingId { get; set; }

        [StringLength(50)]
        public string PhuongThucThanhToan { get; set; }

        public decimal SoTien { get; set; }

        public DateTime NgayThanhToan { get; set; }

        [StringLength(30)]
        public string TrangThai { get; set; }

        [StringLength(100)]
        public string MaGiaoDich { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking DatPhong { get; set; }
    }


    // =====================================================
    // BẢNG ĐÁNH GIÁ
    // =====================================================

    [Table("DanhGia")]
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }

        // Khóa ngoại tới Booking
        public int BookingId { get; set; }

        [Range(1, 5)]
        public int SoSao { get; set; }

        [StringLength(1000)]
        public string BinhLuan { get; set; }

        public DateTime NgayDanhGia { get; set; }

        public bool HoatDong { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking DatPhong { get; set; }
    }
}