
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
     [Table("Company")]
    public class Company
    {
        [Key]
        public Guid CompanyID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public Guid? UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
        public string? Status { get; set; }
    }
}
