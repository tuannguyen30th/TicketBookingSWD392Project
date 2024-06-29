﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("City")]
    public class City
    {
        [Key]
        public Guid CityID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
    }
}
