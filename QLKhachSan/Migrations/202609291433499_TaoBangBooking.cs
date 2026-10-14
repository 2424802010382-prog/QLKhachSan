namespace QLKhachSan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaoBangBooking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        RoomId = c.Int(nullable: false),
                        CustomerName = c.String(nullable: false, maxLength: 100),
                        CustomerPhone = c.String(nullable: false, maxLength: 20),
                        CustomerEmail = c.String(nullable: false, maxLength: 100),
                        CustomerCCCD = c.String(maxLength: 20),
                        CheckInDate = c.DateTime(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false),
                        GuestCount = c.Int(nullable: false),
                        RoomCount = c.Int(nullable: false),
                        SpecialRequest = c.String(),
                        PaymentMethod = c.String(),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .Index(t => t.RoomId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "RoomId", "dbo.Rooms");
            DropIndex("dbo.Bookings", new[] { "RoomId" });
            DropTable("dbo.Bookings");
        }
    }
}
