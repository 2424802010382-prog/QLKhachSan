using QLKhachSan.Models;
using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;  
namespace QLKhachSan.Controllers
{
    public class AccountController : Controller
    {
        private readonly HotelDbContext db =
            new HotelDbContext();


        // =========================================================
        // FORM ĐĂNG NHẬP
        // =========================================================

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        // =========================================================
        // XỬ LÝ ĐĂNG NHẬP
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(
            string username,
            string password
        )
        {
            username =
                (username ?? "")
                .Trim();

            password =
                password ?? "";


            // =====================================================
            // KIỂM TRA DỮ LIỆU
            // =====================================================

            if (
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password)
            )
            {
                TempData["LoginError"] =
                    "Vui lòng nhập đầy đủ tài khoản và mật khẩu.";

                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // CHO PHÉP ĐĂNG NHẬP BẰNG
            // USERNAME HOẶC EMAIL
            // =====================================================

            var user =
                db.Users.FirstOrDefault(
                    u =>
                    (
                        u.Username == username ||
                        u.Email == username
                    )
                    &&
                    u.IsActive
                );


            if (user == null)
            {
                TempData["LoginError"] =
                    "Tên đăng nhập, email hoặc mật khẩu không đúng.";

                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // KIỂM TRA MẬT KHẨU
            // =====================================================

            bool correctPassword =
                false;


            if (
                !string.IsNullOrWhiteSpace(
                    user.PasswordHash
                )
            )
            {
                try
                {
                    // Mật khẩu đã được hash đúng chuẩn
                    correctPassword =
                        Crypto.VerifyHashedPassword(
                            user.PasswordHash,
                            password
                        );
                }
                catch (FormatException)
                {
                    // PasswordHash cũ không đúng định dạng hash
                    correctPassword =
                        false;
                }


                // =================================================
                // HỖ TRỢ TÀI KHOẢN CŨ
                // ĐANG LƯU MẬT KHẨU THƯỜNG
                // =================================================

                if (
                    !correctPassword &&
                    user.PasswordHash == password
                )
                {
                    correctPassword =
                        true;


                    // Chuyển mật khẩu cũ sang hash
                    user.PasswordHash =
                        Crypto.HashPassword(
                            password
                        );


                    db.SaveChanges();
                }
            }


            if (!correctPassword)
            {
                TempData["LoginError"] =
                    "Tên đăng nhập, email hoặc mật khẩu không đúng.";

                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // LƯU SESSION
            // =====================================================

            Session["UserId"] =
                user.UserId;

            Session["Username"] =
                user.Username;

            Session["FullName"] =
                user.FullName;
            Session["Email"] =
                user.Email;

            Session["Role"] =
                user.Role;


            // =====================================================
            // ADMIN
            // =====================================================

            if (user.Role == "Admin")
            {
                return RedirectToAction(
                    "Index",
                    "Admin"
                );
            }


            // =====================================================
            // KHÁCH HÀNG
            // =====================================================

            return RedirectToAction(
                "Index",
                "Home"
            );
        }


        // =========================================================
        // ĐĂNG KÝ TÀI KHOẢN KHÁCH HÀNG
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(
            string fullName,
            string email,
            string password,
            string confirmPassword
        )
        {
            fullName =
                (fullName ?? "")
                .Trim();

            email =
                (email ?? "")
                .Trim()
                .ToLowerInvariant();

            password =
                password ?? "";

            confirmPassword =
                confirmPassword ?? "";


            // =====================================================
            // KIỂM TRA BỎ TRỐNG
            // =====================================================

            if (
                string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword)
            )
            {
                TempData["RegisterError"] =
                    "Vui lòng nhập đầy đủ thông tin đăng ký.";

                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        registerRequired = 1
                    }
                );
            }


            // =====================================================
            // KIỂM TRA EMAIL
            // =====================================================

            try
            {
                var mail =
                    new System.Net.Mail.MailAddress(
                        email
                    );


                if (
                    mail.Address != email
                )
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["RegisterError"] =
                    "Email không đúng định dạng.";

                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        registerRequired = 1
                    }
                );
            }


            // =====================================================
            // KIỂM TRA MẬT KHẨU
            // =====================================================

            if (password.Length < 6)
            {
                TempData["RegisterError"] =
                    "Mật khẩu phải có ít nhất 6 ký tự.";

                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        registerRequired = 1
                    }
                );
            }


            if (password != confirmPassword)
            {
                TempData["RegisterError"] =
                    "Mật khẩu xác nhận không trùng khớp.";

                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        registerRequired = 1
                    }
                );
            }


            // =====================================================
            // KIỂM TRA EMAIL ĐÃ TỒN TẠI
            // =====================================================

            bool userExists =
                db.Users.Any(
                    u =>
                    u.Email == email ||
                    u.Username == email
                );


            if (userExists)
            {
                TempData["RegisterError"] =
                    "Email này đã được sử dụng.";

                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        registerRequired = 1
                    }
                );
            }


            // =====================================================
            // TẠO USER
            // EMAIL ĐƯỢC DÙNG LÀM USERNAME
            // =====================================================

            var user =
                new User
                {
                    Username =
                        email,

                    Email =
                        email,

                    FullName =
                        fullName,

                    PasswordHash =
                        Crypto.HashPassword(
                            password
                        ),

                    Role =
                        "Customer",

                    IsActive =
                        true,

                    CreatedAt =
                        DateTime.Now
                };


            db.Users.Add(
                user
            );


            db.SaveChanges();


            // =====================================================
            // ĐĂNG KÝ THÀNH CÔNG
            // =====================================================

            TempData["RegisterSuccess"] =
                "Đăng ký thành công. Bạn có thể đăng nhập bằng email vừa đăng ký.";


            return RedirectToAction(
                "Index",
                "Home",
                new
                {
                    loginRequired = 1
                }
            );
        }


        // =========================================================
        // ĐĂNG XUẤT
        // =========================================================
        // =========================================================
        // TÀI KHOẢN CỦA TÔI
        // =========================================================

        [HttpGet]
        public ActionResult Profile()
        {
            // =====================================================
            // PHẢI ĐĂNG NHẬP
            // =====================================================

            if (Session["UserId"] == null)
            {
                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // ĐỌC USER ID TỪ SESSION
            // =====================================================

            int userId;


            bool validUserId =
                int.TryParse(
                    Session["UserId"].ToString(),
                    out userId
                );


            if (!validUserId || userId <= 0)
            {
                Session.Clear();


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // TÌM TÀI KHOẢN
            // =====================================================

            var user =
                db.Users.FirstOrDefault(
                    u =>
                    u.UserId == userId &&
                    u.IsActive
                );


            if (user == null)
            {
                Session.Clear();


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // ĐƯA DỮ LIỆU SANG VIEW
            // =====================================================

            var model =
                new AccountProfileViewModel
                {
                    UserId =
                        user.UserId,

                    Username =
                        user.Username,

                    FullName =
                        user.FullName,

                    Email =
                        user.Email
                };


            return View(model);
        }
        // =========================================================
        // CẬP NHẬT THÔNG TIN TÀI KHOẢN
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(
            AccountProfileViewModel model
        )
        {
            // =====================================================
            // PHẢI ĐĂNG NHẬP
            // =====================================================

            if (Session["UserId"] == null)
            {
                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            int userId;


            bool validUserId =
                int.TryParse(
                    Session["UserId"].ToString(),
                    out userId
                );


            if (!validUserId || userId <= 0)
            {
                Session.Clear();


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // CHUẨN HÓA DỮ LIỆU
            // =====================================================

            string fullName =
                (model.FullName ?? "")
                .Trim();


            string email =
                (model.Email ?? "")
                .Trim()
                .ToLowerInvariant();


            // =====================================================
            // KIỂM TRA BỎ TRỐNG
            // =====================================================

            if (
                string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email)
            )
            {
                TempData["ProfileError"] =
                    "Vui lòng nhập đầy đủ họ tên và email.";


                return RedirectToAction(
                    "Profile"
                );
            }


            // =====================================================
            // KIỂM TRA EMAIL
            // =====================================================

            try
            {
                var mail =
                    new System.Net.Mail.MailAddress(
                        email
                    );


                if (mail.Address != email)
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["ProfileError"] =
                    "Email không đúng định dạng.";


                return RedirectToAction(
                    "Profile"
                );
            }


            // =====================================================
            // TÌM USER HIỆN TẠI
            // =====================================================

            var user =
                db.Users.FirstOrDefault(
                    u =>
                    u.UserId == userId &&
                    u.IsActive
                );


            if (user == null)
            {
                Session.Clear();


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // KIỂM TRA EMAIL CÓ BỊ TRÙNG KHÔNG
            // =====================================================

            bool emailExists =
                db.Users.Any(
                    u =>
                    u.UserId != userId &&
                    u.Email == email
                );


            if (emailExists)
            {
                TempData["ProfileError"] =
                    "Email này đang được sử dụng bởi tài khoản khác.";


                return RedirectToAction(
                    "Profile"
                );
            }


            // =====================================================
            // EMAIL CŨ
            // =====================================================

            string oldEmail =
                (user.Email ?? "")
                .Trim()
                .ToLowerInvariant();


            // =====================================================
            // CẬP NHẬT
            // =====================================================

            user.FullName =
                fullName;

            user.Email =
                email;


            // Nếu Username chính là email cũ,
            // đổi Username theo email mới.
            //
            // Nếu Username là tài khoản riêng như "admin"
            // thì giữ nguyên.

            if (
                !string.IsNullOrWhiteSpace(
                    user.Username
                ) &&
                user.Username
                    .Trim()
                    .ToLowerInvariant()
                ==
                oldEmail
            )
            {
                user.Username =
                    email;
            }


            db.SaveChanges();


            // =====================================================
            // CẬP NHẬT SESSION
            // =====================================================

            Session["Username"] =
                user.Username;

            Session["FullName"] =
                user.FullName;

            Session["Email"] =
                user.Email;


            TempData["ProfileSuccess"] =
                "Cập nhật thông tin tài khoản thành công.";


            return RedirectToAction(
                "Profile"
            );
        }
        // =========================================================
        // ĐỔI MẬT KHẨU
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(
            string currentPassword,
            string newPassword,
            string confirmNewPassword
        )
        {
            // =====================================================
            // PHẢI ĐĂNG NHẬP
            // =====================================================

            if (Session["UserId"] == null)
            {
                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // ĐỌC USER ID
            // =====================================================

            int userId;


            bool validUserId =
                int.TryParse(
                    Session["UserId"].ToString(),
                    out userId
                );


            if (!validUserId || userId <= 0)
            {
                Session.Clear();


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // CHUẨN HÓA DỮ LIỆU
            // =====================================================

            currentPassword =
                currentPassword ?? "";

            newPassword =
                newPassword ?? "";

            confirmNewPassword =
                confirmNewPassword ?? "";


            // =====================================================
            // KHÔNG ĐƯỢC BỎ TRỐNG
            // =====================================================

            if (
                string.IsNullOrWhiteSpace(
                    currentPassword
                ) ||
                string.IsNullOrWhiteSpace(
                    newPassword
                ) ||
                string.IsNullOrWhiteSpace(
                    confirmNewPassword
                )
            )
            {
                TempData["PasswordError"] =
                    "Vui lòng nhập đầy đủ thông tin đổi mật khẩu.";


                return RedirectToAction(
                    "Profile"
                );
            }


            // =====================================================
            // MẬT KHẨU MỚI ÍT NHẤT 6 KÝ TỰ
            // =====================================================

            if (newPassword.Length < 6)
            {
                TempData["PasswordError"] =
                    "Mật khẩu mới phải có ít nhất 6 ký tự.";


                return RedirectToAction(
                    "Profile"
                );
            }


            // =====================================================
            // XÁC NHẬN MẬT KHẨU
            // =====================================================

            if (
                newPassword !=
                confirmNewPassword
            )
            {
                TempData["PasswordError"] =
                    "Mật khẩu xác nhận không trùng khớp.";


                return RedirectToAction(
                    "Profile"
                );
            }


            // =====================================================
            // KHÔNG CHO MẬT KHẨU MỚI GIỐNG MẬT KHẨU CŨ
            // =====================================================

            if (
                currentPassword ==
                newPassword
            )
            {
                TempData["PasswordError"] =
                    "Mật khẩu mới phải khác mật khẩu hiện tại.";


                return RedirectToAction(
                    "Profile"
                );
            }


            // =====================================================
            // TÌM USER
            // =====================================================

            var user =
                db.Users.FirstOrDefault(
                    u =>
                    u.UserId == userId &&
                    u.IsActive
                );


            if (user == null)
            {
                Session.Clear();


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        loginRequired = 1
                    }
                );
            }


            // =====================================================
            // KIỂM TRA MẬT KHẨU HIỆN TẠI
            // =====================================================

            bool correctPassword =
                false;


            if (
                !string.IsNullOrWhiteSpace(
                    user.PasswordHash
                )
            )
            {
                try
                {
                    correctPassword =
                        Crypto.VerifyHashedPassword(
                            user.PasswordHash,
                            currentPassword
                        );
                }
                catch (FormatException)
                {
                    // Hỗ trợ dữ liệu cũ
                    // từng lưu mật khẩu dạng thường.

                    correctPassword =
                        user.PasswordHash ==
                        currentPassword;
                }
            }


            if (!correctPassword)
            {
                TempData["PasswordError"] =
                    "Mật khẩu hiện tại không đúng.";


                return RedirectToAction(
                    "Profile"
                );
            }


            // =====================================================
            // HASH VÀ LƯU MẬT KHẨU MỚI
            // =====================================================

            user.PasswordHash =
                Crypto.HashPassword(
                    newPassword
                );


            db.SaveChanges();


            TempData["PasswordSuccess"] =
                "Đổi mật khẩu thành công.";


            return RedirectToAction(
                "Profile"
            );
        }
        // =========================================================
        // QUÊN MẬT KHẨU
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(
            string email
        )
        {
            // =====================================================
            // 1. CHUẨN HÓA EMAIL
            // =====================================================

            email =
                (email ?? "")
                .Trim()
                .ToLowerInvariant();


            // =====================================================
            // 2. KIỂM TRA BỎ TRỐNG
            // =====================================================

            if (
                string.IsNullOrWhiteSpace(
                    email
                )
            )
            {
                TempData["ForgotPasswordError"] =
                    "Vui lòng nhập địa chỉ email.";


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        forgotRequired = 1
                    }
                );
            }


            // =====================================================
            // 3. KIỂM TRA ĐỊNH DẠNG EMAIL
            // =====================================================

            try
            {
                var mail =
                    new System.Net.Mail.MailAddress(
                        email
                    );


                if (
                    mail.Address != email
                )
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["ForgotPasswordError"] =
                    "Email không đúng định dạng.";


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        forgotRequired = 1
                    }
                );
            }


            // =====================================================
            // 4. TÌM TÀI KHOẢN
            // =====================================================

            var user =
                db.Users.FirstOrDefault(
                    u =>
                    u.Email == email &&
                    u.IsActive
                );


            // =====================================================
            // 5. LUÔN HIỆN THÔNG BÁO CHUNG
            //
            // Không nói rõ email có tồn tại hay không,
            // tránh lộ danh sách tài khoản.
            // =====================================================

            TempData["ForgotPasswordMessage"] =
                "Nếu email tồn tại trong hệ thống, yêu cầu đặt lại mật khẩu sẽ được tạo.";


            // =====================================================
            // 6. NẾU KHÔNG CÓ USER
            // =====================================================

            if (user == null)
            {
                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        forgotRequired = 1
                    }
                );
            }


            // =====================================================
            // 7. VÔ HIỆU HÓA TOKEN CŨ CHƯA DÙNG
            // =====================================================

            db.Database.ExecuteSqlCommand(
                @"
            UPDATE dbo.PasswordResetTokens

            SET IsUsed = 1

            WHERE
                UserId = @p0
                AND IsUsed = 0
        ",
                user.UserId
            );


            // =====================================================
            // 8. TẠO TOKEN MỚI
            // =====================================================

            string token =
                GenerateResetToken();


            string tokenHash =
                HashResetToken(
                    token
                );


            DateTime now =
                DateTime.UtcNow;


            DateTime expiresAt =
                now.AddMinutes(30);


            // =====================================================
            // 9. LƯU HASH TOKEN VÀO DATABASE
            //
            // Không lưu token gốc.
            // =====================================================

            db.Database.ExecuteSqlCommand(
                @"
            INSERT INTO dbo.PasswordResetTokens
            (
                UserId,
                TokenHash,
                ExpiresAt,
                IsUsed,
                CreatedAt
            )

            VALUES
            (
                @p0,
                @p1,
                @p2,
                0,
                @p3
            )
        ",
                user.UserId,
                tokenHash,
                expiresAt,
                now
            );


            // =====================================================
            // 10. TẠO LINK RESET
            //
            // TẠM THỜI DÙNG CHO LOCALHOST.
            // Sau này ta sẽ gửi link này qua email thật.
            // =====================================================

            string resetUrl =
                Url.Action(
                    "ResetPassword",
                    "Account",
                    new
                    {
                        token = token
                    },
                    Request.Url.Scheme
                );


            TempData["DevResetUrl"] =
                resetUrl;


            return RedirectToAction(
                "Index",
                "Home",
                new
                {
                    forgotRequired = 1
                }
            );
        }
        // =========================================================
        // TRANG ĐẶT LẠI MẬT KHẨU
        // =========================================================

        [HttpGet]
        public ActionResult ResetPassword(
            string token
        )
        {
            // =====================================================
            // 1. TOKEN PHẢI TỒN TẠI
            // =====================================================

            if (
                string.IsNullOrWhiteSpace(
                    token
                )
            )
            {
                TempData["ForgotPasswordError"] =
                    "Liên kết đặt lại mật khẩu không hợp lệ.";


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        forgotRequired = 1
                    }
                );
            }


            // =====================================================
            // 2. HASH TOKEN TỪ URL
            // =====================================================

            string tokenHash =
                HashResetToken(
                    token
                );


            DateTime now =
                DateTime.UtcNow;


            // =====================================================
            // 3. TÌM TOKEN CÒN HẠN + CHƯA SỬ DỤNG
            // =====================================================

            PasswordResetTokenViewModel
                resetToken =

                db.Database
                  .SqlQuery<PasswordResetTokenViewModel>(
                      @"
                SELECT TOP 1
                    ResetTokenId,
                    UserId,
                    TokenHash,
                    ExpiresAt,
                    IsUsed,
                    CreatedAt

                FROM dbo.PasswordResetTokens

                WHERE
                    TokenHash = @p0
                    AND IsUsed = 0
                    AND ExpiresAt > @p1

                ORDER BY
                    ResetTokenId DESC
              ",
                      tokenHash,
                      now
                  )
                  .FirstOrDefault();


            // =====================================================
            // 4. TOKEN KHÔNG HỢP LỆ
            // =====================================================

            if (resetToken == null)
            {
                TempData["ForgotPasswordError"] =
                    "Liên kết đặt lại mật khẩu đã hết hạn hoặc không hợp lệ.";


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        forgotRequired = 1
                    }
                );
            }


            // =====================================================
            // 5. ĐƯA TOKEN GỐC SANG VIEW
            // =====================================================

            var model =
                new ResetPasswordViewModel
                {
                    Token =
                        token
                };


            return View(model);
        }
        // =========================================================
        // XỬ LÝ ĐẶT LẠI MẬT KHẨU
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(
            ResetPasswordViewModel model
        )
        {
            // =====================================================
            // 1. KIỂM TRA MODEL
            // =====================================================

            if (model == null)
            {
                TempData["ForgotPasswordError"] =
                    "Dữ liệu đặt lại mật khẩu không hợp lệ.";


                return RedirectToAction(
                    "Index",
                    "Home",
                    new
                    {
                        forgotRequired = 1
                    }
                );
            }


            model.Token =
                model.Token ?? "";

            model.NewPassword =
                model.NewPassword ?? "";

            model.ConfirmNewPassword =
                model.ConfirmNewPassword ?? "";


            // =====================================================
            // 2. KIỂM TRA BỎ TRỐNG
            // =====================================================

            if (
                string.IsNullOrWhiteSpace(
                    model.Token
                ) ||
                string.IsNullOrWhiteSpace(
                    model.NewPassword
                ) ||
                string.IsNullOrWhiteSpace(
                    model.ConfirmNewPassword
                )
            )
            {
                ViewBag.Error =
                    "Vui lòng nhập đầy đủ thông tin.";


                return View(model);
            }


            // =====================================================
            // 3. MẬT KHẨU MỚI ÍT NHẤT 6 KÝ TỰ
            // =====================================================

            if (
                model.NewPassword.Length < 6
            )
            {
                ViewBag.Error =
                    "Mật khẩu mới phải có ít nhất 6 ký tự.";


                return View(model);
            }


            // =====================================================
            // 4. XÁC NHẬN MẬT KHẨU
            // =====================================================

            if (
                model.NewPassword !=
                model.ConfirmNewPassword
            )
            {
                ViewBag.Error =
                    "Mật khẩu xác nhận không trùng khớp.";


                return View(model);
            }


            // =====================================================
            // 5. HASH TOKEN
            // =====================================================

            string tokenHash =
                HashResetToken(
                    model.Token
                );


            DateTime now =
                DateTime.UtcNow;


            // =====================================================
            // 6. KIỂM TRA TOKEN LẦN NỮA
            //
            // POST cũng phải kiểm tra lại,
            // không được chỉ tin GET.
            // =====================================================

            PasswordResetTokenViewModel
                resetToken =

                db.Database
                  .SqlQuery<PasswordResetTokenViewModel>(
                      @"
                SELECT TOP 1
                    ResetTokenId,
                    UserId,
                    TokenHash,
                    ExpiresAt,
                    IsUsed,
                    CreatedAt

                FROM dbo.PasswordResetTokens

                WHERE
                    TokenHash = @p0
                    AND IsUsed = 0
                    AND ExpiresAt > @p1

                ORDER BY
                    ResetTokenId DESC
              ",
                      tokenHash,
                      now
                  )
                  .FirstOrDefault();


            if (resetToken == null)
            {
                ViewBag.Error =
                    "Liên kết đặt lại mật khẩu đã hết hạn hoặc đã được sử dụng.";


                return View(model);
            }


            // =====================================================
            // 7. TÌM USER
            // =====================================================

            var user =
                db.Users.FirstOrDefault(
                    u =>
                    u.UserId ==
                    resetToken.UserId &&
                    u.IsActive
                );


            if (user == null)
            {
                ViewBag.Error =
                    "Không thể đặt lại mật khẩu cho tài khoản này.";


                return View(model);
            }


            // =====================================================
            // 8. TRANSACTION
            // =====================================================

            using (
                var transaction =
                    db.Database.BeginTransaction()
            )
            {
                try
                {
                    // =============================================
                    // HASH MẬT KHẨU MỚI
                    // =============================================

                    user.PasswordHash =
                        Crypto.HashPassword(
                            model.NewPassword
                        );


                    db.SaveChanges();


                    // =============================================
                    // ĐÁNH DẤU TOKEN ĐÃ DÙNG
                    // =============================================

                    db.Database.ExecuteSqlCommand(
                        @"
                    UPDATE dbo.PasswordResetTokens

                    SET IsUsed = 1

                    WHERE
                        ResetTokenId = @p0
                ",
                        resetToken.ResetTokenId
                    );


                    // =============================================
                    // VÔ HIỆU HÓA TẤT CẢ TOKEN KHÁC CỦA USER
                    // =============================================

                    db.Database.ExecuteSqlCommand(
                        @"
                    UPDATE dbo.PasswordResetTokens

                    SET IsUsed = 1

                    WHERE
                        UserId = @p0
                        AND IsUsed = 0
                ",
                        user.UserId
                    );


                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();


                    ViewBag.Error =
                        "Có lỗi xảy ra khi đặt lại mật khẩu. Vui lòng thử lại.";


                    return View(model);
                }
            }


            // =====================================================
            // 9. XÓA SESSION HIỆN TẠI TRÊN TRÌNH DUYỆT NÀY
            // =====================================================

            Session.Clear();

            Session.Abandon();


            // =====================================================
            // 10. QUAY VỀ LOGIN
            // =====================================================

            TempData["LoginSuccess"] =
                "Đặt lại mật khẩu thành công. Hãy đăng nhập bằng mật khẩu mới.";


            return RedirectToAction(
                "Index",
                "Home",
                new
                {
                    loginRequired = 1
                }
            );
        }
        public ActionResult Logout()
        {
            Session.Clear();

            Session.Abandon();


            return RedirectToAction(
                "Index",
                "Home"
            );
        }


        // =========================================================
        // GIẢI PHÓNG DATABASE CONTEXT
        // =========================================================
        // =========================================================
        // TẠO RESET TOKEN NGẪU NHIÊN
        // =========================================================

        private static string GenerateResetToken()
        {
            byte[] bytes =
                new byte[32];


            using (
                RandomNumberGenerator rng =
                    RandomNumberGenerator.Create()
            )
            {
                rng.GetBytes(bytes);
            }


            // Chuyển Base64 thành dạng an toàn cho URL
            return Convert
                .ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }


        // =========================================================
        // HASH RESET TOKEN BẰNG SHA-256
        // =========================================================

        private static string HashResetToken(
            string token
        )
        {
            if (
                string.IsNullOrWhiteSpace(token)
            )
            {
                return "";
            }


            using (
                SHA256 sha256 =
                    SHA256.Create()
            )
            {
                byte[] inputBytes =
                    Encoding.UTF8.GetBytes(
                        token
                    );


                byte[] hashBytes =
                    sha256.ComputeHash(
                        inputBytes
                    );


                StringBuilder builder =
                    new StringBuilder();


                foreach (
                    byte value
                    in hashBytes
                )
                {
                    builder.Append(
                        value.ToString("x2")
                    );
                }


                return builder.ToString();
            }
        }
        protected override void Dispose(
            bool disposing
        )
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}