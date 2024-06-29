
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Service")]
    public class Service
    {
        [Key]
        public Guid ServiceID { get; set; }
      /*  public int RouteID { get; set; }
        [ForeignKey("RouteID")]
        public Route Route { get; set; }*/
        public Guid? ServiceTypeID { get; set; }
        [ForeignKey("ServiceTypeID")]
        public ServiceType? ServiceType { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

    }
}
