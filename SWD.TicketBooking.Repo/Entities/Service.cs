
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Service")]
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }
      /*  public int RouteID { get; set; }
        [ForeignKey("RouteID")]
        public Route Route { get; set; }*/
        public int ServiceTypeID { get; set; }
        [ForeignKey("ServiceTypeID")]
        public ServiceType ServiceType { get; set; }
        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string UrlGuidID { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

    }
}
