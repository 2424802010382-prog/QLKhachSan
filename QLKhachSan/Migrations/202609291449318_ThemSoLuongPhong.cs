namespace QLKhachSan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemSoLuongPhong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "TotalRooms", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rooms", "TotalRooms");
        }
    }
}
