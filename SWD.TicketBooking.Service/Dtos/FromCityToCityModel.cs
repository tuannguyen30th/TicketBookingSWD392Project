using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class FromCityToCityModel
    {
        public class CityInfo
        {
            public int CityID { get; set; }
            public string CityName { get; set; }
        }

        public class CityModel
        {
            public List<CityInfo> FromCities { get; set; }
            public List<CityInfo> ToCities { get; set; }
        }

    }
}
