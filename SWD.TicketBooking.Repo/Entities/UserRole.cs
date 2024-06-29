using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public Guid RoleID { get; set; }

        [Required]
        [StringLength(50)]
        public string? RoleName { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

    }
}
