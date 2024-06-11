
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
     [Table("Company")]
    public class Company
    {
        [Key]
        public Guid CompanyID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Status { get; set; }
    }
}
