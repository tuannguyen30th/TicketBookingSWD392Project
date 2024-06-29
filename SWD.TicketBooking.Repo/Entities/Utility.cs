using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Utility")]
    public class Utility
    {
        
        [Key]
        public Guid UtilityID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

    }
}
