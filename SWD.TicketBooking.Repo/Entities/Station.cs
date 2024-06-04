using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Station")]
    public class Station
    {
        [Key]
        public int StationID { get; set; }
        public int CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }
        public int CityID { get; set; }
        [ForeignKey("CityID")]
        public City City { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

    }


}
