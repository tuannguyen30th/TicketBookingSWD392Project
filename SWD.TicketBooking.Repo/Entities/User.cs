using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserID { get; set; }

        [MaxLength(255)]
        public string? UserName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Password { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? FullName { get; set; } = string.Empty;

        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Avatar { get; set; } = string.Empty;


        [MaxLength(255)]
        public string? Address { get; set; } = string.Empty;

        public string? OTPCode { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? PhoneNumber { get; set; } = string.Empty;
        //public double Balance { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public bool? IsVerified { get; set; }
        public string? AccessToken { get; set; } = string.Empty;

        public DateTime? TokenExpiration { get; set; }
        //public string? AccessDevice { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public Guid? RoleID { get; set; }

        [ForeignKey("RoleID")]

        public UserRole UserRole { get; set; }
        public Guid? CompanyID { get; set; }
        [ForeignKey("CompanyID")]

        public Company Company { get; set; }

        public bool IsTokenExpired()
        {
            return DateTime.UtcNow > TokenExpiration;
        }
    }
}
