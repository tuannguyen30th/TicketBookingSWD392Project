using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Trip")]
    public class Trip
    {
        [Key]
        public Guid TripID { get; set; }
        public Guid? Route_CompanyID { get; set; }
        [ForeignKey("Route_CompanyID")]
        public Route_Company? Route_Company { get; set; }
        public Guid? StaffID { get; set; }
        [ForeignKey("StaffID")]
        public User? User { get; set; }
        public bool? IsTemplate { get; set; }
        public Guid? TemplateID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
/*        public string ImageUrl { get; set; } = string.Empty;*/
        public string? Status { get; set; } = string.Empty;

        // nếu IsTemplate thì không được để null Template
        // 1 api tạo chuyến gốc, 1 api tạo chuyến copy
    }
}
