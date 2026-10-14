namespace QLKhachSan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemBangDichVuKhuyenMaiThanhToanDanhGia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhGia",
                c => new
                    {
                        MaDanhGia = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        SoSao = c.Int(nullable: false),
                        BinhLuan = c.String(maxLength: 1000),
                        NgayDanhGia = c.DateTime(nullable: false),
                        HoatDong = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MaDanhGia)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.BookingId);
            
            CreateTable(
                "dbo.DatPhongDichVu",
                c => new
                    {
                        MaDatPhongDichVu = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        MaDichVu = c.Int(nullable: false),
                        SoLuong = c.Int(nullable: false),
                        DonGia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ThanhTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MaDatPhongDichVu)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .ForeignKey("dbo.DichVu", t => t.MaDichVu, cascadeDelete: true)
                .Index(t => t.BookingId)
                .Index(t => t.MaDichVu);
            
            CreateTable(
                "dbo.DichVu",
                c => new
                    {
                        MaDichVu = c.Int(nullable: false, identity: true),
                        TenDichVu = c.String(nullable: false, maxLength: 100),
                        MoTa = c.String(maxLength: 500),
                        Gia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonVi = c.String(maxLength: 50),
                        HoatDong = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MaDichVu);
            
            CreateTable(
                "dbo.KhuyenMai",
                c => new
                    {
                        MaKhuyenMai = c.Int(nullable: false, identity: true),
                        MaCode = c.String(nullable: false, maxLength: 30),
                        TenKhuyenMai = c.String(nullable: false, maxLength: 100),
                        MoTa = c.String(maxLength: 500),
                        LoaiGiamGia = c.String(nullable: false, maxLength: 20),
                        GiaTriGiam = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonToiThieu = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GiamToiDa = c.Decimal(precision: 18, scale: 2),
                        NgayBatDau = c.DateTime(nullable: false),
                        NgayKetThuc = c.DateTime(nullable: false),
                        SoLuotSuDungToiDa = c.Int(),
                        SoLuotDaSuDung = c.Int(nullable: false),
                        HoatDong = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MaKhuyenMai);
            
            CreateTable(
                "dbo.ThanhToan",
                c => new
                    {
                        MaThanhToan = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        PhuongThucThanhToan = c.String(maxLength: 50),
                        SoTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NgayThanhToan = c.DateTime(nullable: false),
                        TrangThai = c.String(maxLength: 30),
                        MaGiaoDich = c.String(maxLength: 100),
                        GhiChu = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.MaThanhToan)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.BookingId);
            
            AlterColumn("dbo.Bookings", "CustomerName", c => c.String());
            AlterColumn("dbo.Bookings", "CustomerPhone", c => c.String());
            AlterColumn("dbo.Bookings", "CustomerEmail", c => c.String());
            AlterColumn("dbo.Bookings", "CustomerCCCD", c => c.String());
            AlterColumn("dbo.Rooms", "RoomName", c => c.String());
            AlterColumn("dbo.Users", "Username", c => c.String());
            AlterColumn("dbo.Users", "PasswordHash", c => c.String());
            AlterColumn("dbo.Users", "FullName", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "Role", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThanhToan", "BookingId", "dbo.Bookings");
            DropForeignKey("dbo.DatPhongDichVu", "MaDichVu", "dbo.DichVu");
            DropForeignKey("dbo.DatPhongDichVu", "BookingId", "dbo.Bookings");
            DropForeignKey("dbo.DanhGia", "BookingId", "dbo.Bookings");
            DropIndex("dbo.ThanhToan", new[] { "BookingId" });
            DropIndex("dbo.DatPhongDichVu", new[] { "MaDichVu" });
            DropIndex("dbo.DatPhongDichVu", new[] { "BookingId" });
            DropIndex("dbo.DanhGia", new[] { "BookingId" });
            AlterColumn("dbo.Users", "Role", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 100));
            AlterColumn("dbo.Users", "FullName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "PasswordHash", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Rooms", "RoomName", c => c.String(nullable: false));
            AlterColumn("dbo.Bookings", "CustomerCCCD", c => c.String(maxLength: 20));
            AlterColumn("dbo.Bookings", "CustomerEmail", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Bookings", "CustomerPhone", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Bookings", "CustomerName", c => c.String(nullable: false, maxLength: 100));
            DropTable("dbo.ThanhToan");
            DropTable("dbo.KhuyenMai");
            DropTable("dbo.DichVu");
            DropTable("dbo.DatPhongDichVu");
            DropTable("dbo.DanhGia");
        }
    }
}
