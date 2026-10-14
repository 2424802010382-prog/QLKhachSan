using System.Data.Entity;

namespace QLKhachSan.Models
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext()
            : base("HotelDbContext")
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DichVu> DichVus { get; set; }

        public DbSet<KhuyenMai> KhuyenMais { get; set; }

        public DbSet<DatPhongDichVu> DatPhongDichVus { get; set; }

        public DbSet<ThanhToan> ThanhToans { get; set; }

        public DbSet<DanhGia> DanhGias { get; set; }
    }
}