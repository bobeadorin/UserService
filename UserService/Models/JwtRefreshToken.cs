using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    public class JwtRefreshToken
    {
        public Guid Id { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public DateTime ExpirationDate { get; set; }    
        public bool IsExpired { get; set; } = false;
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
    }
}
