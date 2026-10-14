using System;

namespace QLKhachSan.Models
{
    public class PasswordResetTokenViewModel
    {
        public int ResetTokenId { get; set; }

        public int UserId { get; set; }

        public string TokenHash { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}