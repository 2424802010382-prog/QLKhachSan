namespace QLKhachSan.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QLKhachSan.Models.HotelDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QLKhachSan.Models.HotelDbContext context)
        {
            context.Rooms.AddOrUpdate(
    r => r.RoomName,

    new QLKhachSan.Models.Room
    {
        RoomName = "Phòng Deluxe",
        RoomType = "DELUXE",
        Price = 2000000,
        Capacity = 2,
        TotalRooms = 5,

        Description = "Phòng nghỉ sang trọng với nội thất hiện đại.",
        ImageUrl = "https://images.unsplash.com/photo-1566665797739-1674de7a421a?q=80&w=900",
        IsActive = true
    },

    new QLKhachSan.Models.Room
    {
        RoomName = "Phòng Premium",
        RoomType = "PREMIUM",
        Price = 2500000,
        Capacity = 2,
        TotalRooms = 5,

        Description = "Phòng cao cấp với không gian rộng rãi.",
        ImageUrl = "https://images.unsplash.com/photo-1611892440504-42a792e24d32?q=80&w=900",
        IsActive = true
    },

    new QLKhachSan.Models.Room
    {
        RoomName = "Luxury Balcony Suite",
        RoomType = "SUITE",
        Price = 3500000,
        Capacity = 2,
        TotalRooms = 5,

        Description = "Phòng suite sang trọng có ban công.",
        ImageUrl = "https://images.unsplash.com/photo-1590490360182-c33d57733427?q=80&w=900",
        IsActive = true
    },

    new QLKhachSan.Models.Room
    {
        RoomName = "Phòng Tổng Thống",
        RoomType = "PRESIDENT",
        Price = 8000000,
        Capacity = 4,
        TotalRooms = 5,

        Description = "Phòng cao cấp nhất của khách sạn.",
        ImageUrl = "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?q=80&w=900",
        IsActive = true
    },

    new QLKhachSan.Models.Room
    {
        RoomName = "Phòng Standard",
        RoomType = "STANDARD",
        Price = 800000,
        Capacity = 2,
        TotalRooms = 5,

        Description = "Phòng tiêu chuẩn đầy đủ tiện nghi.",
        ImageUrl = "https://images.unsplash.com/photo-1618773928121-c32242e63f39?q=80&w=900",
        IsActive = true
    },

    new QLKhachSan.Models.Room
    {
        RoomName = "Phòng Superior",
        RoomType = "SUPERIOR",
        Price = 1200000,
        Capacity = 2,
        TotalRooms = 5,

        Description = "Phòng rộng rãi, thoải mái.",
        ImageUrl = "https://images.unsplash.com/photo-1590490359683-658d3d23f972?q=80&w=900",
        IsActive = true
    },

    new QLKhachSan.Models.Room
    {
        RoomName = "Phòng Twin",
        RoomType = "TWIN ROOM",
        Price = 1000000,
        Capacity = 2,
        TotalRooms = 5,

        Description = "Phòng gồm hai giường đơn.",
        ImageUrl = "https://images.unsplash.com/photo-1595576508898-0ad5c879a061?q=80&w=900",
        IsActive = true
    },

    new QLKhachSan.Models.Room
    {
        RoomName = "Phòng Family",
        RoomType = "FAMILY ROOM",
        Price = 1500000,
        Capacity = 4,
        TotalRooms = 5,

        Description = "Phòng phù hợp cho gia đình.",
        ImageUrl = "https://images.unsplash.com/photo-1590490360182-c33d57733427?q=80&w=900",
        IsActive = true
    }


);
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                context.Users.Add(new QLKhachSan.Models.User
                {
                    Username = "admin",

                    PasswordHash =
                        System.Web.Helpers.Crypto.HashPassword("123456"),

                    FullName = "Quản trị viên",

                    Email = "admin@luxuryhotel.com",

                    Role = "Admin",

                    IsActive = true,

                    CreatedAt = DateTime.Now
                });
            }

            if (!context.DichVus.Any())
            {
                context.DichVus.Add(
                    new QLKhachSan.Models.DichVu
                    {
                        TenDichVu = "Buffet sáng",
                        MoTa = "Buffet sáng tại nhà hàng của khách sạn.",
                        Gia = 150000,
                        DonVi = "người",
                        HoatDong = true
                    }
                );

                context.DichVus.Add(
                    new QLKhachSan.Models.DichVu
                    {
                        TenDichVu = "Spa thư giãn",
                        MoTa = "Gói spa thư giãn và chăm sóc cơ thể.",
                        Gia = 500000,
                        DonVi = "lần",
                        HoatDong = true
                    }
                );

                context.DichVus.Add(
                    new QLKhachSan.Models.DichVu
                    {
                        TenDichVu = "Đưa đón sân bay",
                        MoTa = "Dịch vụ đưa hoặc đón khách tại sân bay.",
                        Gia = 300000,
                        DonVi = "lượt",
                        HoatDong = true
                    }
                );

                context.DichVus.Add(
                    new QLKhachSan.Models.DichVu
                    {
                        TenDichVu = "Giặt ủi",
                        MoTa = "Dịch vụ giặt và ủi quần áo.",
                        Gia = 80000,
                        DonVi = "kg",
                        HoatDong = true
                    }
                );
            }


            // ==========================================================
            // DỮ LIỆU MẪU KHUYẾN MÃI
            // ==========================================================

            if (!context.KhuyenMais.Any())
            {
                context.KhuyenMais.Add(
                    new QLKhachSan.Models.KhuyenMai
                    {
                        MaCode = "WELCOME10",
                        TenKhuyenMai = "Ưu đãi chào mừng",
                        MoTa = "Giảm 10% tiền phòng, tối đa 500.000đ.",
                        LoaiGiamGia = "PhanTram",
                        GiaTriGiam = 10,
                        DonToiThieu = 1000000,
                        GiamToiDa = 500000,

                        NgayBatDau =
                            new DateTime(2026, 1, 1),

                        NgayKetThuc =
                            new DateTime(2026, 12, 31),

                        SoLuotSuDungToiDa = 100,
                        SoLuotDaSuDung = 0,
                        HoatDong = true
                    }
                );


                context.KhuyenMais.Add(
                    new QLKhachSan.Models.KhuyenMai
                    {
                        MaCode = "VIP500",
                        TenKhuyenMai = "Ưu đãi khách VIP",

                        MoTa =
                            "Giảm trực tiếp 500.000đ cho đơn từ 5.000.000đ.",

                        LoaiGiamGia = "SoTien",
                        GiaTriGiam = 500000,
                        DonToiThieu = 5000000,
                        GiamToiDa = null,

                        NgayBatDau =
                            new DateTime(2026, 1, 1),

                        NgayKetThuc =
                            new DateTime(2026, 12, 31),

                        SoLuotSuDungToiDa = 50,
                        SoLuotDaSuDung = 0,
                        HoatDong = true
                    }
                );
            }


            // ==========================================================
            // LƯU DỮ LIỆU
            // ==========================================================

            context.SaveChanges();
        }
    }
}
