/* =============================================
   THÊM USER VÀ KHUYẾN MÃI VÀO BOOKING
   ============================================= */

IF COL_LENGTH('dbo.Bookings', 'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.Bookings
    ADD UserId INT NULL;
END


IF COL_LENGTH('dbo.Bookings', 'MaKhuyenMai') IS NULL
BEGIN
    ALTER TABLE dbo.Bookings
    ADD MaKhuyenMai INT NULL;
END


IF COL_LENGTH('dbo.Bookings', 'TienGiam') IS NULL
BEGIN
    ALTER TABLE dbo.Bookings
    ADD TienGiam DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Bookings_TienGiam DEFAULT 0;
END


/* =============================================
   KHÓA NGOẠI BOOKING → USER
   ============================================= */

IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Bookings_Users'
)
BEGIN
    ALTER TABLE dbo.Bookings

    ADD CONSTRAINT FK_Bookings_Users

    FOREIGN KEY (UserId)

    REFERENCES dbo.Users(UserId);
END


/* =============================================
   KHÓA NGOẠI BOOKING → KHUYẾN MÃI
   ============================================= */

IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Bookings_KhuyenMai'
)
BEGIN
    ALTER TABLE dbo.Bookings

    ADD CONSTRAINT FK_Bookings_KhuyenMai

    FOREIGN KEY (MaKhuyenMai)

    REFERENCES dbo.KhuyenMai(MaKhuyenMai);
END