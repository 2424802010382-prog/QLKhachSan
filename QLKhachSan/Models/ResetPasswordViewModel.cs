namespace QLKhachSan.Models
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}